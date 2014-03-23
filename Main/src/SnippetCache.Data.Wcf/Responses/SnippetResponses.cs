using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SnippetCache.Data.DTO;

namespace SnippetCache.Data.Wcf.Responses
{
    [DataContract]
    public class GetPagedSnippetListResponse : ServiceResponseBase
    {
        [DataMember(IsRequired = true)]
        public IEnumerable<SnippetDTO> Snippets { get; set; }
    }

    [DataContract]
    public class GetSnippetByIdResponse : ServiceResponseBase
    {
        [DataMember(IsRequired = true)]
        public SnippetDTO Snippet { get; set; }
    }

    [DataContract]
    public class GetSnippetsByUserIdResponse : ServiceResponseBase
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<SnippetDTO> Snippets { get; set; }
    }

    [DataContract]
    public class CreateSnippetResponse : ServiceResponseBase
    {
        [DataMember]
        public int SnippetId { get; set; }
    }

    [DataContract]
    public class UpdateSnippetResponse : ServiceResponseBase
    {
        [DataMember]
        public ISnippet Snippet { get; set; }
    }

    [DataContract]
    public class DeleteSnippetResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class GetSnippetByGuidResponse : ServiceResponseBase
    {
        [DataMember(IsRequired = true)] public SnippetDTO Snippet;
    }

    [DataContract]
    public class GetSnippetsByTitleResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<SnippetDTO> Snippets { get; set; }
    }

    [DataContract]
    public class GetSnippetsByDescriptionResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<SnippetDTO> Snippets { get; set; }
    }

    [DataContract]
    public class GetSnippetsByDataResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<SnippetDTO> Snippets { get; set; }
    }
}