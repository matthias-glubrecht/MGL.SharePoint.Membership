using MGL.SharePoint.Membership.AD;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace MGL.SharePoint.Membership
{
    static class SPHelper
    {
        public static List<DomainAccount> GetSiteGroupMembers(SPSite site, string groupName)
        {
            List<DomainAccount> members = new List<DomainAccount>();
            try
            {
                SPGroup group = site.RootWeb.SiteGroups[groupName];
                foreach (SPUser user in group.Users)
                {
                    DomainAccount domainAccount = new DomainAccount(user);
                    members.Add(domainAccount);
                }
            }
            catch
            {
                throw;
            }

            return members;
        }

        internal static List<string> GetAllSiteGroups(Guid siteId)
        {
            var names = new List<string>();
            using (SPSite site = new SPSite(siteId))
            {
                foreach (SPGroup group in site.RootWeb.SiteGroups)
                {
                    names.Add(group.Name);
                }
            }
            return names;
        }

        internal static List<string> GetSiteGroupsOfUser(Guid siteId, string loginName)
        {
            var groupNames = new List<string>();
            List<DomainAccount> adGroupsOfUser = null;
            using (SPSite site = new SPSite(siteId))
            {
                site.RootWeb.AllowUnsafeUpdates = true;
                SPUser user = site.RootWeb.EnsureUser(loginName);
                foreach (SPGroup group in site.RootWeb.SiteGroups)
                {
                    foreach (SPUser groupMember in group.Users)
                    {
                        if (groupMember.IsDomainGroup)
                        {
                            var adGroup = new DomainAccount(groupMember);
                            if (adGroupsOfUser == null)
                            {
                                adGroupsOfUser = ADHelper.GetUserAdGroups(loginName);
                            }
                            if (adGroupsOfUser.Exists(g => g.Equals(adGroup)))
                            {
                                groupNames.Add(group.Name);
                                break;
                            }
                            else
                            {
                                if (adGroup.IsAllAuthenticatedUsers || adGroup.IsEveryone)
                                {
                                    groupNames.Add(group.Name);
                                    break;
                                }
                            }
                        }
                        else if (groupMember.ID == user.ID)
                        {
                            groupNames.Add(group.Name);
                            break;
                        }
                    }
                }
            }
            return groupNames;
        }
    }
}
