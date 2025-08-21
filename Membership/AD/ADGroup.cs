#region Legal Disclaimer
/*
 * This Sample Code is provided for the purpose of illustration only and is not
 * intended to be used in a production environment.
 * THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT 
 * WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED
 * TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR 
 * PURPOSE.  
 * We grant You a nonexclusive, royalty-free right to use and modify the Sample 
 * Code and to reproduce and distribute the object code form of the Sample 
 * Code, provided that You agree: 
 *   (i) to not use Our name, logo, or trademarks to market Your software 
 *       product in which the Sample Code is embedded; 
 *  (ii) to include a valid copyright notice on Your software product in which
 *       the Sample Code is embedded; and 
 * (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and 
 *       against any claims or lawsuits, including attorneys’ fees, that arise 
 *       or result from the use or distribution of the Sample Code.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.Serialization;

namespace MGL.SharePoint.Membership.AD
{
    /// <summary>
    /// A simple wrapper for an AD group for serialization.
    /// </summary>
    [DataContract]
    public class ADGroup
    {
#region DataMembers
        [DataMember]
        public int MemberCount { get; set; }

        [DataMember]
        public DomainAccount Account { get; set; }

        [DataMember]
        public List<AdUser> Users { get; set; }
        
        [DataMember]
        public string ErrorMessages { get; set; }
        
        [DataMember]
        public List<string> Log { get; private set; }
#endregion

        private ADGroup() { }

        public ADGroup(DomainAccount account, bool resolveUsers)
        {
            Account = account;
            Log = new List<string>();
            Users = new List<AdUser>();
            if (resolveUsers)
            {
                Resolve();
            }
        }

        private void Resolve()
        {
            try
            {
                Dictionary<String, AdUser> userDict = new Dictionary<string, AdUser>();
                Log.Add($"Reading members of AD group '{Account.LoginName}'.");
                PrincipalContext context = new PrincipalContext(ContextType.Domain, Account.Domain);

                GroupPrincipal principal = null;
                if (!String.IsNullOrEmpty(Account.SamAccountName))
                {
                    Log.Add($"Using SamAccountName {Account.SamAccountName} to find group.");
                    principal = GroupPrincipal.FindByIdentity(context, IdentityType.SamAccountName, Account.SamAccountName);
                }
                else if (!String.IsNullOrEmpty(Account.Sid))
                {
                    Log.Add($"Using Sid {Account.Sid} to find group.");
                    principal = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, Account.Sid);
                }
                else if (!String.IsNullOrEmpty(Account.DisplayName))
                {
                    Log.Add($"Using DisplayName {Account.DisplayName} to find group.");
                    principal = GroupPrincipal.FindByIdentity(context, IdentityType.Name, Account.DisplayName);
                }
                else
                {
                    Log.Add($"Using LoginName {Account.LoginName} to find group.");
                    principal = GroupPrincipal.FindByIdentity(context, IdentityType.Name, Account.LoginName);
                }
                if (principal != null)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    PrincipalSearchResult<Principal> searchResult = principal.GetMembers(true);
                    sw.Stop();
                    Log.Add(String.Format("AD Query took {0} seconds.", sw.Elapsed.TotalSeconds));
                    sw.Reset();
                    sw.Start();
                    foreach (Principal member in searchResult)
                    {
                        if (member.StructuralObjectClass == "user")
                        {
                            string key = member.Sid.ToString();
                            userDict[key] = new AdUser(member);
                        }
                        else
                        {
                            Debugger.Break();
                        }
                    }
                    sw.Stop();
                    Log.Add(String.Format("{0} members resolved, this took {1} seconds.", userDict.Values.Count, sw.Elapsed.TotalSeconds));
                }
                Users = new List<AdUser>(userDict.Values.OrderBy(user => user.Login));
                MemberCount = userDict.Values.Count;
            }
            catch (Exception ex)
            {
                ErrorMessages = ex.GetAllMessages("<br/>");
            }
        }
    }
}
