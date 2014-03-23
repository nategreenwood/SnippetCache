using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Requests
{
    [DataContract]
    public class GetPagedSnippetListRequest
    {
        [DataMember]
        public int SnippetsPerPage { get; set; }

        [DataMember]
        public int CurrentPage { get; set; }

        [DataMember(IsRequired = false)]
        public bool IncludePrivate { get; set; }

        [DataMember(IsRequired = false)]
        public int UserId { get; set; }

        [DataMember(IsRequired = false)]
        public Guid UserGuid { get; set; }
    }

    [DataContract]
    public class GetSnippetByIdRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    public class GetSnippetByGuidRequest
    {
        [DataMember(IsRequired = true)] public Guid Guid;
    }

    [DataContract]
    public class GetSnippetsByUserIdRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public Guid User_FormsAuthId { get; set; }
    }

    [DataContract]
    public class CreateSnippetRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public byte[] PreviewData { get; set; }

        [DataMember]
        public byte[] Data { get; set; }

        [DataMember]
        public DateTime LastModified { get; set; }

        [DataMember]
        public bool IsPublic { get; set; }

        [DataMember]
        public int Language_Id { get; set; }

        [DataMember]
        public int User_Id { get; set; }

        [DataMember]
        public Guid User_FormsAuthId { get; set; }
    }

    [DataContract]
    public class UpdateSnippetRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public byte[] PreviewData { get; set; }

        [DataMember]
        public byte[] Data { get; set; }

        [DataMember]
        public DateTime LastModified { get; set; }

        [DataMember]
        public bool IsPublic { get; set; }

        [DataMember]
        public int Language_Id { get; set; }

        [DataMember]
        public int User_Id { get; set; }

        [DataMember]
        public Guid User_FormsAuthId { get; set; }
    }

    [DataContract]
    public class DeleteSnippetRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    public class GetSnippetsByTitleRequest
    {
        [DataMember(IsRequired = true)]
        public string TitleText { get; set; }

        [DataMember(IsRequired = false)]
        public int UserId { get; set; }

        [DataMember(IsRequired = false)]
        public Guid UserGuid { get; set; }
    }

    [DataContract]
    public class GetSnippetsByDescriptionRequest
    {
        [DataMember(IsRequired = true)]
        public string DescriptionText { get; set; }

        [DataMember(IsRequired = false)]
        public int UserId { get; set; }

        [DataMember(IsRequired = false)]
        public Guid UserGuid { get; set; }
    }

    [DataContract]
    public class GetSnippetsByDataRequest
    {
        [DataMember(IsRequired = true)]
        public string DataString { get; set; }

        [DataMember(IsRequired = false)]
        public int UserId { get; set; }

        [DataMember(IsRequired = false)]
        public Guid UserGuid { get; set; }
    }
}