using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.DTO
{
    [DataContract]
    public class NotificationDTO : INotification
    {
        #region INotification Members

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public int NotificationType_Id { get; set; }

        [DataMember]
        public int User_Id { get; set; }

        [DataMember]
        public Guid User_FormsAuthId { get; set; }

        [DataMember]
        public int Id { get; set; }

        #endregion
    }
}