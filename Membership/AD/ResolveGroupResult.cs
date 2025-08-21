using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MGL.SharePoint.Membership.AD
{
    [DataContract]
    public class ResolveGroupResult
    {
        [DataMember]
        public ADGroup Group { get; set; }
        
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
