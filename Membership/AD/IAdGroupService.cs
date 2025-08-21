using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using MGL.SharePoint.Membership.AD;

namespace BITS_ISO_CM
{
    [ServiceContract]
    interface IAdGroupService
    {
        [OperationContract]
        [WebGet(UriTemplate = "ResolveGroup/{groupName}",
            ResponseFormat = WebMessageFormat.Json)]
        ResolveGroupResult ResolveGroup(string groupName);
    }
}
