using System.ServiceModel;
using System.ServiceModel.Web;

namespace MGL.SharePoint.Membership
{
    [ServiceContract]
    interface IMembershipService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetAllSiteGroups", ResponseFormat = WebMessageFormat.Json)]
        StringListResultWrapper GetAllSiteGroups();

        [OperationContract]
        [WebGet(UriTemplate = "IsUserInSiteGroup?login={loginName}&group={groupName}", ResponseFormat = WebMessageFormat.Json)]
        BoolResultWrapper IsUserInSiteGroup(string loginName, string groupName);

        [OperationContract]
        [WebGet(UriTemplate = "GetSiteGroupMembers?group={groupName}", ResponseFormat = WebMessageFormat.Json)]
        StringListResultWrapper GetSiteGroupMembers(string groupName);

        [OperationContract]
        [WebGet(UriTemplate = "GetADGroupMembers?group={adGroupName}", ResponseFormat = WebMessageFormat.Json)]
        StringListResultWrapper GetADGroupMembers(string adGroupName);

        [OperationContract]
        [WebGet(UriTemplate = "IsUserInADGroup?login={loginName}&group={adGroupName}", ResponseFormat = WebMessageFormat.Json)]
        BoolResultWrapper IsUserInADGroup(string loginName, string adGroupName);

        [OperationContract]
        [WebGet(UriTemplate = "GetADGroupsOfUser?login={loginName}", ResponseFormat = WebMessageFormat.Json)]
        StringListResultWrapper GetADGroupsOfUser(string loginName);

        [OperationContract]
        [WebGet(UriTemplate = "GetSiteGroupsOfUser?login={loginName}", ResponseFormat = WebMessageFormat.Json)]
        StringListResultWrapper GetSiteGroupsOfUser(string loginName);

        [OperationContract]
        [WebGet(UriTemplate = "ResolveADGroup?group={adGroupName}", ResponseFormat = WebMessageFormat.Json)]
        ADGroupResultWrapper ResolveADGroup(string adGroupName);

    }
}
