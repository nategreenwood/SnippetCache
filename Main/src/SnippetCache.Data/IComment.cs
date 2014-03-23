using System;

namespace SnippetCache.Data
{
    public interface IComment : IBaseEntity
    {
        string Text { get; set; }
        DateTime LastModified { get; set; }
        int SnippetId { get; set; }
        int UserId { get; set; }
        Guid UserFormsAuthId { get; set; }
    }
}