using System;
using System.Collections.Generic;
using SnippetCache.Data.DTO;

namespace SnippetCache.Managers
{
    public interface INotificationManager : IManager
    {
        IEnumerable<NotificationDTO> UnreadUserNotification(int userId, Guid userGuid);
    }
}