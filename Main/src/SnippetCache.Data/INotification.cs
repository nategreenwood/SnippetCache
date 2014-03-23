using System;

namespace SnippetCache.Data
{
    public interface INotification : IBaseEntity
    {
        string Text { get; set; }
        DateTime DateCreated { get; set; }
        int NotificationType_Id { get; set; }
        int User_Id { get; set; }
        Guid User_FormsAuthId { get; set; }
    }
}