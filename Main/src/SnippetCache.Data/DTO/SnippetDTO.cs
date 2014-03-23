using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnippetCache.Data.DTO
{
    [DataContract]
    public class SnippetDTO : ISnippet, IEqualityComparer<SnippetDTO>
    {
        #region ISnippet Members

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

        [DataMember]
        public int Id { get; set; }

        #endregion

        #region Implementation of IEqualityComparer<in SnippetDTO>

        public bool Equals(SnippetDTO x, SnippetDTO y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(SnippetDTO obj)
        {
            return obj.Id.GetHashCode();
        }

        #endregion
    }
}