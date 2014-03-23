using System.Collections.Generic;
using System.Runtime.Serialization;
using SnippetCache.Data.Wcf.Requests;

namespace SnippetCache.Data.Wcf.Responses
{
    [DataContract]
    public class DeleteNotificationTypesResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class UpdateNotificationTypesResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class CreateNewNotificationTypesResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class GetNotificationTypesByNameResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class GetNotificationTypesByIdResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class GetAllNotificationTypesResponse : ServiceResponseBase
    {
        [DataMember] public IEnumerable<NotificationTypeQuickInfo> NotificationTypes;
    }
}