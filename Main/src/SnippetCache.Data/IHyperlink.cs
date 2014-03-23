using System;

namespace SnippetCache.Data
{
    public interface IHyperlink : IBaseEntity
    {
        string Uri { get; set; }
        string Description { get; set; }
        DateTime LastModified { get; set; }
        int Snippet_Id { get; set; }
    }
}