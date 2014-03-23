using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.DTO
{
    [DataContract]
    public class UserDTO : IUser
    {
        #region IUser Members

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid FormsAuthId { get; set; }

        [DataMember]
        public string LoginName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public byte[] AvatarImage { get; set; }

        #endregion
    }
}