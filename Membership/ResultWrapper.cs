using MGL.SharePoint.Membership.AD;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MGL.SharePoint.Membership
{
    [DataContract]
    public class StringListResultWrapper
    {
        [DataMember]
        public bool Success { get; private set; }
        [DataMember]
        public string Error { get; private set; }
        [DataMember]
        public List<string> Result { get; private set; }
        public StringListResultWrapper(List<String> result, Exception ex)
        {
            Result = result;
            if (ex != null)
            {
                Success = false;
                Error = ex.GetAllMessages(" ");
            }
            else
            {
                Success = true;
            }
        }

        public StringListResultWrapper(string errorMessage)
        {
            this.Error = errorMessage;
            this.Result = new List<string>();
            this.Success = false;
        }
    }

    [DataContract]
    public class ADGroupResultWrapper
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public ADGroup Result { get; set; }
        public ADGroupResultWrapper()
        {
            Result = null;
            Success = false;
            Error = null;
        }
    }

    [DataContract]
    public class BoolResultWrapper
    {
        [DataMember]
        public bool Success { get; private set; }
        [DataMember]
        public string Error { get; private set; }
        [DataMember]
        public bool Result { get; private set; }
        public BoolResultWrapper(bool result, Exception ex)
        {
            if (ex != null)
            {
                Success = false;
                Error = ex.GetAllMessages(" ");
            }
            else
            {
                Success = true;
                Result = result;
            }
        }
    }
}
