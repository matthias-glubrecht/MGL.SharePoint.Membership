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
using System.DirectoryServices.AccountManagement;
using System.Runtime.Serialization;
using System.Xml;

namespace MGL.SharePoint.Membership.AD
{
    /// <summary>
    /// A simple wrapper for an AD User
    /// </summary>
    [DataContract]
    public class AdUser
    {
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string DisplayName { get; set; }

        private AdUser() { }

        public AdUser(DomainAccount da)
        {
            Login = da.LoginName;
            DisplayName = Fix(da.DisplayName);
        }

        public AdUser(Principal principal)
        {
            Login = String.Format("{0}\\{1}", principal.Context.Name, principal.SamAccountName);
            DisplayName = Fix(principal.DisplayName);
        }

        /// <summary>
        /// Entfernen von "invalid characters".
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Fix(string text)
        {
            if (text == null)
            {
                return String.Empty;
            }
            foreach (char c in text.ToCharArray())
            {
                if (c < 0x20)
                {
                    text = text.Replace(new String(c, 1), String.Empty);
                }
            }
            return text;
        }
    }
}
