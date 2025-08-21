using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGL.SharePoint.Membership
{
    public static class Extensions
    {
        public static string RemoveClaimsPrefixIfPresent(this string claimsLogin)
        {
            if (claimsLogin.IndexOf('|') != -1)
            {
                return claimsLogin.Split('|')[1];
            }
            return claimsLogin;
        }

        public static string GetAllMessages(this Exception ex, string delimiter)
        {
            StringBuilder sb = new StringBuilder(ex.Message);
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                sb.Append(delimiter);
                sb.Append(ex.Message);
            }
            return sb.ToString();
        }

        public static bool HasDomain(this string loginName)
        {
            return loginName.IndexOf('\\') != -1;
        }
        public static string RemoveDomain(this string loginName)
        {
            if (loginName.HasDomain())
            {
                var parts = loginName.Split('\\');
                return parts[parts.Length - 1];
            }
            return loginName;
        }

        public static bool HasPermissionsAssigned(this SPPrincipal principal, SPWeb web, SPRoleDefinition roleDefinition)
        {
            SPRoleAssignment roleAssignment = web.RoleAssignments.GetAssignmentByPrincipal(principal);
            foreach (SPRoleDefinition roleDefinitionBinding in roleAssignment.RoleDefinitionBindings)
            {
                if (roleDefinitionBinding == roleDefinition)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
