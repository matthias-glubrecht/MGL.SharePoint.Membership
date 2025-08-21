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
using Microsoft.SharePoint;
using System.Runtime.Serialization;

namespace MGL.SharePoint.Membership.AD
{
    /// <summary>
    /// A wrapper class for Users and AD groups encountered in SharePoint RoleAssignments.
    /// </summary>
    [DataContract]
    public class DomainAccount
    {
        public const string CLAIM_EVERYONE = "c:0(.s|true";
        public const string CLAIM_ALL_AUTHENTICATED_USERS = "c:0!.s|windows";

        [DataMember]
        public bool IsEveryone { get; set; }

        [DataMember]
        public bool IsAllAuthenticatedUsers { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Domain { get; set; }
        
        [DataMember]
        public string SamAccountName { get; set; }
        
        [DataMember]
        public string Email { get; set; }
        
        [DataMember]
        public string DisplayName { get; set; }
        
        [DataMember]
        public string LoginName { get; set; }

        [DataMember]
        public string Sid { get; set; }
        private DomainAccount() {
            IsEveryone = IsAllAuthenticatedUsers = false;
        }

        public DomainAccount(string loginName)
        {
            UserId = -1;
            LoginName = loginName;
            if (LoginName == CLAIM_EVERYONE)
            {
                IsEveryone = true;
            }
            else if (LoginName == CLAIM_ALL_AUTHENTICATED_USERS)
            {
                IsAllAuthenticatedUsers = true;
            }
            else
            {
                LoginName = LoginName.RemoveClaimsPrefixIfPresent();
                if (ADHelper.IsSID(LoginName))
                {
                    Sid = LoginName;
                    LoginName = ADHelper.ResolveSID(LoginName);
                }
                string[] parts = LoginName.Split('\\');
                if (parts.Length == 2)
                {
                    Domain = parts[0];
                    SamAccountName = parts[1];
                }
                else
                {
                    Domain = String.Empty;
                    SamAccountName = LoginName;
                }
                Email = String.Empty;
                DisplayName = LoginName;
            }
        }

        public DomainAccount(SPUser spUser) : this()
        {
            UserId = spUser.ID;
            Sid = spUser.Sid;
            LoginName = spUser.LoginName;
            if (LoginName == CLAIM_EVERYONE)
            {
                IsEveryone = true;
            }
            else if (LoginName == CLAIM_ALL_AUTHENTICATED_USERS)
            {
                IsAllAuthenticatedUsers = true;
            }
            else
            {
                LoginName = LoginName.RemoveClaimsPrefixIfPresent();
                if (!ADHelper.IsSID(LoginName))
                {
                    string[] parts = LoginName.Split('\\');
                    if (parts.Length == 2)
                    {
                        Domain = parts[0];
                        SamAccountName = parts[1];
                    }
                }
                else // LoginName ist eine SID
                {
                    if (String.IsNullOrEmpty(Sid))
                    {
                        Sid = LoginName;
                        LoginName = ADHelper.ResolveSID(LoginName);
                    }
                }
                if (String.IsNullOrEmpty(Domain))
                {
                    string[] parts = spUser.Name.Split('\\');
                    if (parts.Length == 2)
                    {
                        Domain = parts[0];
                        SamAccountName = parts[1];
                    }
                }
            }
            Email = spUser.Email;
            DisplayName = spUser.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            DomainAccount other = obj as DomainAccount;
            return 
                !String.IsNullOrEmpty(LoginName) && LoginName.Equals(other.LoginName, StringComparison.InvariantCultureIgnoreCase)
                ||
                !String.IsNullOrEmpty(SamAccountName) && SamAccountName.Equals(other.SamAccountName, StringComparison.InvariantCultureIgnoreCase)
                ||
                !String.IsNullOrEmpty(Sid) && Sid.Equals(other.Sid, StringComparison.InvariantCultureIgnoreCase)
                ||
                !String.IsNullOrEmpty(DisplayName) && DisplayName.Equals(other.DisplayName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (String.IsNullOrEmpty(LoginName) ? 0 : LoginName.ToLowerInvariant().GetHashCode()) +
                (String.IsNullOrEmpty(SamAccountName) ? 0 : SamAccountName.ToLowerInvariant().GetHashCode()) +
                (String.IsNullOrEmpty(Sid) ? 0 : Sid.ToLowerInvariant().GetHashCode()) +
                (String.IsNullOrEmpty(DisplayName) ? 0 : DisplayName.ToLowerInvariant().GetHashCode());
        }

        public override string ToString()
        {
            return LoginName;
        }
    }
}
