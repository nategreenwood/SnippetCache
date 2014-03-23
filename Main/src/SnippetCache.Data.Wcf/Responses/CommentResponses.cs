using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Responses
{
    [DataContract]
    public class GetAllCommentsResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<CommentQuickInfo> Comments { get; set; }
    }

    [DataContract]
    public class GetAllCommentsForSnippetResponse : ServiceResponseBase
    {
        [DataMember]
        public int SnippetId { get; set; }

        [DataMember]
        public IEnumerable<CommentQuickInfo> Comments { get; set; }
    }

    [DataContract]
    public class GetCommentByIdResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

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
    public class CreateNewCommentResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

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
    public class UpdateCommentResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public DateTime DateLastModified { get; set; }
    }

    [DataContract]
    public class DeleteCommentResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    public class CommentQuickInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public DateTime DataLastModified { get; set; }
    }
}