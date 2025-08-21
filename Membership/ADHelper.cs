using MGL.SharePoint.Membership.AD;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace MGL.SharePoint.Membership
{
    static class ADHelper
    {
        static readonly ConcurrentDictionary<string, string> _sidDict;
        static ADHelper()
        {
            int numProcs = Environment.ProcessorCount;
            int concurrencyLevel = numProcs * 2;

            // Construct the dictionary with the desired concurrencyLevel and initialCapacity
            _sidDict = new ConcurrentDictionary<string, string>(concurrencyLevel, 100);
        }

        public static List<DomainAccount> GetUserAdGroups(string userName)
        {
            userName = userName.RemoveClaimsPrefixIfPresent();
            var groups = new List<DomainAccount>();
            using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal principal = UserPrincipal.FindByIdentity(domainContext, userName);
                if (principal != null)
                {
                    using (DirectoryEntry user = principal.GetUnderlyingObject() as DirectoryEntry)
                    {
                        if (user != null)
                        {
                            groups.AddRange(GetGroupsFromADObject(user));
                        }
                    }
                }
                else
                {
                    throw new Exception($"Benutzer {userName} nicht gefunden.");
                }
            }
            return groups;
        }

        // Internal helper function for reading the "tokenGroups" from
        // a "DirectoryEntry" (user account)
        private static List<DomainAccount> GetGroupsFromADObject(DirectoryEntry user)
        {
            var groups = new List<DomainAccount>();
            if (user != null)
            {
                try
                {
                    // try to load object data and check type (class)
                    user.RefreshCache(new string[] { "class", "tokenGroups" });
                    string cl = user.SchemaClassName;
                    if (cl == "user")
                    {
                        // check if "tokenGroups" attribute is filled
                        PropertyValueCollection tg = user.Properties["tokenGroups"];

                        if (tg != null)
                        {
                            Debug.WriteLine(String.Format("User '{0}' is member of {1} groups.", user.Name, tg.Count));

                            try
                            {
                                // everything fine -> process data
                                foreach (byte[] SID in (Array)tg.Value)
                                {
                                    // Convert the SID into SDDL-format
                                    string sddlSid = InteropHelper.ConvertSidToSDDL(SID);
                                    if (groups.Find(d =>  d.Sid == sddlSid) == null)
                                    {
                                        groups.Add(new DomainAccount(sddlSid));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(String.Format("Error converting SID from TokenGroups attribute '{0}'.", tg.Value), ex);
                            }
                        }
                        else
                        {
                            // attribute empty -> access denied!
                            throw new FieldAccessException("Access was denied on the \"tokenGroups\" attribute!");
                        }
                    }
                    else
                    {
                        // object is not a user object!
                        throw new NotSupportedException("The object is not a user account!");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to determine AD groups for user '{0}'", user.Name), ex);
                }
            }
            else
            {
                throw new ArgumentNullException("user", "Can not read groups for AD user.");
            }
            return groups;
        }

        public static bool IsSID(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || !s.ToLowerInvariant().StartsWith("s-"))
            {
                return false;
            }
            try 
            { 
                SecurityIdentifier sid = new SecurityIdentifier(s);
                return true;
            } 
            catch (ArgumentException) 
            { 
                return false; 
            } 
            catch (SystemException)
            { 
                return false;
            }
        }

        public static string ResolveSID(string sid)
        {
            if (_sidDict.ContainsKey(sid))
            {
                return _sidDict[sid];
            }
            else
            {
                string account = InteropHelper.LookupAccount(sid, "");
                _sidDict.TryAdd(sid, account);
                return account;
            }
        }
    }
}
