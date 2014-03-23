using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Responses
{
    [DataContract]
    public class ServiceResponseBase
    {
        public ServiceResponseBase()
        {
            Success = false;
            FailureInformation = string.Empty;
        }

        public ServiceResponseBase(bool success, string failureInfo)
        {
            Success = success;
            FailureInformation = failureInfo;
        }

        [DataMember(IsRequired = true)]
        public bool Success { get; set; }

        [DataMember(IsRequired = true)]
        public string FailureInformation { get; set; }
    }
}