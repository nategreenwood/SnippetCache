using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Requests
{
    [DataContract]
    public class DeleteNotificationTypesRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    public class UpdateNotificationTypesRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class CreateNewNotificationTypesRequest
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class GetNotificationTypesByNameRequest
    {
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class GetNotificationTypesByIdRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    public class GetAllNotificationTypesRequest
    {
        // No op
    }

    [DataContract]
    public class NotificationTypeQuickInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}