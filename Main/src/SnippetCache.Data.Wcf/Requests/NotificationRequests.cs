using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Requests
{
    [DataContract]
    public class DeleteNotificationsRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    public class UpdateNotificationsRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int Notification_Type_Id { get; set; }

        [DataMember]
        public int User_Id { get; set; }

        [DataMember]
        public Guid User_Forms_AuthId { get; set; }
    }

    [DataContract]
    public class CreateNewNotificationsRequest
    {
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int Notification_Type_Id { get; set; }

        [DataMember]
        public int User_Id { get; set; }

        [DataMember]
        public Guid User_Forms_AuthId { get; set; }
    }

    public class GetNotificationsByNameRequest
    {
        // No op
    }

    [DataContract]
    public class GetNotificationsByIdRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    public class GetAllNotificationsRequest
    {
        // No op
    }

    [DataContract]
    public class GetAllNotificationsForUserRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public Guid UserGuid { get; set; }
    }
}