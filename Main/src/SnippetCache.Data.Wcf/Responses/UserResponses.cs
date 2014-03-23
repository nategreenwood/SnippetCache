using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Responses
{
    [DataContract]
    public class DeleteUserResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class UpdateUserResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class CreateNewUserResponse : ServiceResponseBase
    {
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
    }

    [DataContract]
    public class GetUserByNameResponse : ServiceResponseBase
    {
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
    }

    [DataContract]
    public class GetUserByIdResponse : ServiceResponseBase
    {
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
    }

    [DataContract]
    public class GetAllUsersResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<UserQuickInfo> Users { get; set; }
    }

    [DataContract]
    public class UserQuickInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid FormsAuthId { get; set; }

        [DataMember]
        public string LoginName { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}