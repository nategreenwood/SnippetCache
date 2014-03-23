using System;

namespace SnippetCache.Data
{
    public interface IUser : IBaseEntity
    {
        Guid FormsAuthId { get; set; }
        string LoginName { get; set; }
        string Email { get; set; }
        byte[] AvatarImage { get; set; }
    }
}