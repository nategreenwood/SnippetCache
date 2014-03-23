using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Requests
{
    [DataContract]
    public class DeleteUserRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid FormsAuthId { get; set; }
    }

    [DataContract]
    public class CreateNewUserRequest
    {
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
    public class UpdateUserRequest
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
    public class GetUserByNameRequest
    {
        [DataMember]
        public string LoginName { get; set; }
    }

    [DataContract]
    public class GetUserByIdRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid FormsAuthId { get; set; }
    }

    [DataContract]
    public class GetAllUsersRequest
    {
        // No op
    }
}