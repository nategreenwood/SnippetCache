using System.Collections.Generic;
using System.Runtime.Serialization;
using SnippetCache.Data.DTO;

namespace SnippetCache.Data.Wcf.Responses
{
    [DataContract]
    public class GetHyperlinksBySnippetIdResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<HyperlinkDTO> Hyperlinks { get; set; }
    }

    [DataContract]
    public class GetHyperlinkByIdResponse : ServiceResponseBase
    {
        [DataMember]
        public HyperlinkDTO Hyperlink { get; set; }
    }

    [DataContract]
    public class GetHyperlinksByUserIdResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<HyperlinkDTO> Hyperlinks { get; set; }
    }

    [DataContract]
    public class CreateHyperlinkResponse : ServiceResponseBase
    {
        [DataMember]
        public int HyperlinkId { get; set; }
    }

    [DataContract]
    public class UpdateHyperlinkResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class DeleteHyperlinkResponse : ServiceResponseBase
    {
        // No op
    }
}