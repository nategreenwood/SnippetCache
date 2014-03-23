using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Requests
{
    [DataContract]
    public class GetHyperlinksBySnippetIdRequest
    {
        [DataMember(IsRequired = true)]
        public int UserId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid UserGuid { get; set; }

        [DataMember(IsRequired = true)]
        public Guid SnippetId { get; set; }
    }

    [DataContract]
    public class GetHyperlinkByIdRequest
    {
        [DataMember(IsRequired = true)]
        public int HyperlinkId { get; set; }
    }

    [DataContract]
    public class GetHyperlinksByUserIdRequest
    {
        [DataMember(IsRequired = true)]
        public int UserId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid UserGuid { get; set; }

        [DataMember(IsRequired = true)]
        public int SnippetId { get; set; }
    }

    [DataContract]
    public class CreateHyperlinkRequest
    {
        [DataMember(IsRequired = true)]
        public string Uri { get; set; }

        [DataMember(IsRequired = false)]
        public string Description { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime LastModified { get; set; }

        [DataMember(IsRequired = true)]
        public int SnippetId { get; set; }
    }

    [DataContract]
    public class UpdateHyperlinkRequest
    {
        [DataMember(IsRequired = true)]
        public int HyperlinkId { get; set; }

        [DataMember(IsRequired = true)]
        public string Uri { get; set; }

        [DataMember(IsRequired = false)]
        public string Description { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime LastModified { get; set; }
    }

    [DataContract]
    public class DeleteHyperlinkRequest
    {
        [DataMember(IsRequired = true)]
        public int HyperlinkId { get; set; }
    }
}