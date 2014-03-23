using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.DTO
{
    [DataContract]
    public class FileDTO : IFile
    {
        #region IFile Members

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public byte[] Data { get; set; }

        [DataMember]
        public DateTime LastModified { get; set; }

        [DataMember]
        public int Snippet_Id { get; set; }

        [DataMember]
        public int Id { get; set; }

        #endregion
    }
}