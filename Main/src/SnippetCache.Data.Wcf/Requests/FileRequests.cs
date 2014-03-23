using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Requests
{
    [DataContract]
    public class GetFilesBySnippetIdRequest
    {
        [DataMember(IsRequired = true)]
        public int UserId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid UserGuid { get; set; }

        [DataMember(IsRequired = true)]
        public Guid SnippetId { get; set; }
    }

    [DataContract]
    public class GetFileByIdRequest
    {
        [DataMember(IsRequired = true)]
        public int FileId { get; set; }
    }

    [DataContract]
    public class GetFilesByUserIdRequest
    {
        [DataMember(IsRequired = true)]
        public int UserId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid UserGuid { get; set; }

        [DataMember(IsRequired = true)]
        public int SnippetId { get; set; }
    }

    [DataContract]
    public class CreateFileRequest
    {
        [DataMember(IsRequired = true)]
        public int UserId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid UserGuid { get; set; }

        [DataMember(IsRequired = true)]
        public int SnippetId { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = false)]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public byte[] Data { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime LastModified { get; set; }
    }

    [DataContract]
    public class UpdateFileRequest
    {
        [DataMember(IsRequired = true)]
        public int UserId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid UserGuid { get; set; }

        [DataMember(IsRequired = true)]
        public int SnippetId { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = false)]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public byte[] Data { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime LastModified { get; set; }
    }

    [DataContract]
    public class DeleteFileRequest
    {
        [DataMember]
        public int FileId { get; set; }
    }
}