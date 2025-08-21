using MGL.SharePoint.Membership;
using MGL.SharePoint.Membership.AD;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel.Activation;

namespace MGL.SharePoint
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class MembershipService : IMembershipService
    {
        const string CATEGORY = "MembershipService";

        public BoolResultWrapper IsUserInADGroup(string loginName, string adGroupName)
        {
            bool returnValue = false;
            try
            {
                EnsureParameter("login", loginName);
                EnsureParameter("group", adGroupName);

                var domainAccount = new DomainAccount(adGroupName);
                if (domainAccount.IsEveryone || domainAccount.IsAllAuthenticatedUsers)
                {
                    returnValue = true;
                }
                else
                {
                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        var adGroups = ADHelper.GetUserAdGroups(loginName);
                        returnValue = adGroups.Exists(g => g.Equals(domainAccount));
                    });
                }
            }
            catch (Exception ex)
            {
                return new BoolResultWrapper(false, ex);
            }
            return new BoolResultWrapper(returnValue, null);
        }

        public BoolResultWrapper IsUserInSiteGroup(string loginName, string groupName)
        {
            var siteId = SPContext.Current.Site.ID;
            try
            {
                EnsureParameter("login", loginName);
                EnsureParameter("group", groupName);
                bool returnValue = false;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    {
                        site.RootWeb.AllowUnsafeUpdates = true;
                        var spUser = site.RootWeb.EnsureUser(loginName);
                        var userAccount = new DomainAccount(spUser);
                        var groupMembers = SPHelper.GetSiteGroupMembers(site, groupName);
                        
                        Debug.WriteLine($"members of {groupName}: {String.Join(", ", groupMembers.ConvertAll(m => m.DisplayName))}", CATEGORY);
                        
                        if (groupMembers.Exists(m => m.IsAllAuthenticatedUsers || m.IsEveryone))
                        {
                            returnValue = true;
                        }
                        else if (groupMembers.Exists(m => m.Equals(userAccount)))
                        {
                            returnValue = true;
                        }
                        else
                        {
                            var adGroups = ADHelper.GetUserAdGroups(userAccount.LoginName);

                            foreach (var adGroup in adGroups)
                            {
                                if (groupMembers.Exists(m => m.Equals(adGroup)))
                                {
                                    returnValue = true;
                                    break;
                                }
                            }
                        }
                    }
                });
                return new BoolResultWrapper(returnValue, null);
            }
            catch (Exception ex)
            {
                return new BoolResultWrapper(false, ex);
            }
        }

        public StringListResultWrapper GetADGroupsOfUser(string loginName)
        {
            List<string> returnValue = new List<string>();
            try
            {
                EnsureParameter("login", loginName);
                loginName = loginName.RemoveClaimsPrefixIfPresent();
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    returnValue = ADHelper.GetUserAdGroups(loginName).ConvertAll(dm => dm.LoginName);
                });

                return new StringListResultWrapper(returnValue, null);
            }
            catch (Exception ex)
            {
                return new StringListResultWrapper(returnValue, ex);
            }
        }

        public StringListResultWrapper GetAllSiteGroups()
        {
            var siteId = SPContext.Current.Site.ID;
            List<string> returnValue = new List<string>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    returnValue = SPHelper.GetAllSiteGroups(siteId);
                });

                return new StringListResultWrapper(returnValue, null);
            }
            catch (Exception ex)
            {
                return new StringListResultWrapper(returnValue, ex);
            }
        }

        public StringListResultWrapper GetSiteGroupsOfUser(string loginName)
        {
            var siteId = SPContext.Current.Site.ID;
            List<string> returnValue = new List<string>();
            try
            {
                EnsureParameter("login", loginName);
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    returnValue = SPHelper.GetSiteGroupsOfUser(siteId, loginName);
                });

                return new StringListResultWrapper(returnValue, null);
            }
            catch (Exception ex)
            {
                return new StringListResultWrapper(returnValue, ex);
            }
        }

        public StringListResultWrapper GetSiteGroupMembers(string groupName)
        {
            var siteId = SPContext.Current.Site.ID;
            List<string> returnValue = new List<string>();
            try
            {
                EnsureParameter("group", groupName);
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        returnValue = SPHelper.GetSiteGroupMembers(site, groupName).ConvertAll(dm => dm.LoginName);
                    }
                });

                return new StringListResultWrapper(returnValue, null);
            }
            catch (Exception ex)
            {
                return new StringListResultWrapper(returnValue, ex);
            }

        }

        public ADGroupResultWrapper ResolveADGroup(string adGroupName)
        {
            ADGroupResultWrapper wrapper = new ADGroupResultWrapper();
            try
            {
                EnsureParameter("group", adGroupName);
                var siteId = SPContext.Current.Site.ID;
                var webId = SPContext.Current.Web.ID;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPUser user = web.EnsureUser(adGroupName);
                        if (user.IsDomainGroup)
                        {
                            wrapper.Result = new ADGroup(new DomainAccount(user), true);
                            wrapper.Success = true;
                        }
                        else
                        {
                            wrapper.Error = String.Format("{0} is not the name of an AD group.", adGroupName);
                            wrapper.Success = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                wrapper.Error = ex.GetAllMessages(Environment.NewLine);
                wrapper.Success = false;
            }
            return wrapper;
        }

        public StringListResultWrapper GetADGroupMembers(string adGroupName)
        {
            var result = ResolveADGroup(adGroupName);
            if (result.Success)
            {
                return new StringListResultWrapper(result.Result.Users.ConvertAll<string>(u => u.Login), null);
            }
            else
            {
                return new StringListResultWrapper(result.Error);
            }
        }

        private static void EnsureParameter(string parameterName, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"Parameter '{parameterName}' missing.");
            }
        }
    }
}
