using System;

namespace SnippetCache.Data
{
    public interface IFile : IBaseEntity
    {
        string Name { get; set; }
        string Description { get; set; }
        byte[] Data { get; set; }
        DateTime LastModified { get; set; }
        int Snippet_Id { get; set; }
    }
}