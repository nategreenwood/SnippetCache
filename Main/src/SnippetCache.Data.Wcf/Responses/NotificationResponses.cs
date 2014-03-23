using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Responses
{
    public class DeleteNotificationsResponse : ServiceResponseBase
    {
        // No op
    }

    public class UpdateNotificationsResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class CreateNewNotificationsResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int NotificationTypeId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public Guid UserFormsAuthId { get; set; }
    }

    public class GetNotificationsByNameResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class GetNotificationsByIdResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int NotificationTypeId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public Guid UserFormsAuthId { get; set; }
    }

    [DataContract]
    public class GetAllNotificationsResponse : ServiceResponseBase
    {
        [DataMember] public IEnumerable<NotificationQuickInfo> Notifications;
    }

    [DataContract]
    public class GetAllNotificationsForUserResponse : ServiceResponseBase
    {
        [DataMember] public IEnumerable<NotificationQuickInfo> Notifications;
    }

    [DataContract]
    public class NotificationQuickInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }
    }
}