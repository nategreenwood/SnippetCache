using System;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Requests
{
    [DataContract]
    public class GetAllCommentsRequest
    {
        // No op
    }

    [DataContract]
    public class GetAllCommentsForSnippetRequest
    {
        [DataMember]
        public int SnippetId { get; set; }
    }

    [DataContract]
    public class GetCommentByIdRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    public class CreateNewCommentRequest
    {
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public DateTime DateLastModified { get; set; }

        [DataMember]
        public int SnippetId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public Guid UserFormsAuthId { get; set; }
    }

    [DataContract]
    public class UpdateCommentRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public DateTime DateLastModified { get; set; }
    }

    [DataContract]
    public class DeleteCommentRequest
    {
        [DataMember]
        public int Id { get; set; }
    }
}