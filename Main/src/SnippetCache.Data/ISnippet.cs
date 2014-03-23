using System;

namespace SnippetCache.Data
{
    public interface ISnippet : IBaseEntity
    {
        Guid Guid { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        byte[] PreviewData { get; set; }
        byte[] Data { get; set; }
        DateTime LastModified { get; set; }
        bool IsPublic { get; set; }
        int Language_Id { get; set; }
        int User_Id { get; set; }
        Guid User_FormsAuthId { get; set; }
    }
}