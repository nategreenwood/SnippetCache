using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.DTO
{
    public class HyperlinkDTO : IHyperlink
    {
        #region IHyperlink Members

        [DataMember]
        public string Uri { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime LastModified { get; set; }

        [DataMember]
        public int Snippet_Id { get; set; }

        [DataMember]
        public int Id { get; set; }

        #endregion
    }
}