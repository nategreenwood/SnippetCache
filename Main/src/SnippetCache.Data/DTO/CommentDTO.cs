using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.DTO
{
    [DataContract]
    public class CommentDTO : IComment
    {
        #region IComment Members

        [DataMember]
        public string Text { get; set; }

        public DateTime LastModified { get; set; }

        [DataMember]
        public int SnippetId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public Guid UserFormsAuthId { get; set; }

        [DataMember]
        public int Id { get; set; }

        #endregion
    }
}