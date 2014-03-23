using System;
using System.Collections.Generic;
using System.Linq;
using SnippetCache.Data.DTO;
using SnippetCache.Engines.DataService;
using SnippetCache.Utils.Validation;

namespace SnippetCache.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly ISnippetCacheDataService _dataService;

        public NotificationManager()
        {
            if (_dataService == null)
                _dataService = new SnippetCacheDataServiceClient();
        }

        public NotificationManager(ISnippetCacheDataService dataService)
        {
            _dataService = dataService;
        }

        #region INotificationManager Members

        public string ManagerTypeName
        {
            get { return "NotificationManager"; }
        }

        public IEnumerable<NotificationDTO> UnreadUserNotification(int userId, Guid userGuid)
        {
            Guard.IntArgIsPositive(userId, "userId");
            Guard.GuidArgIsValue(userGuid, "userGuid");

            GetAllNotificationsForUserResponse result =
                _dataService.GetNotificationsForUser(new GetAllNotificationsForUserRequest
                                                         {
                                                             UserId = userId,
                                                             UserGuid = userGuid
                                                         });
            var notificationDtos = new List<NotificationDTO>(result.Notifications.Count());
            notificationDtos.AddRange(result.Notifications.Select(
                notificationQuickInfo => new NotificationDTO
                                             {
                                                 Id = notificationQuickInfo.Id,
                                                 DateCreated = notificationQuickInfo.DateCreated,
                                                 Text = notificationQuickInfo.Text
                                             }));

            return notificationDtos.AsEnumerable();
        }

        #endregion

        public bool AddUserNotification(NotificationDTO notification)
        {
            Guard.ArgNotNull(notification, "notification");

            CreateNewNotificationsResponse result = _dataService.CreateNewNotification(new CreateNewNotificationsRequest
                                                                                           {
                                                                                               Text = notification.Text,
                                                                                               User_Id =
                                                                                                   notification.User_Id,
                                                                                               User_Forms_AuthId =
                                                                                                   notification.
                                                                                                   User_FormsAuthId,
                                                                                               Notification_Type_Id =
                                                                                                   notification.
                                                                                                   NotificationType_Id
                                                                                           });
            return result.Success;
        }

        public bool DeleteUserNotification(NotificationDTO notification)
        {
            Guard.ArgNotNull(notification, "notification");
            DeleteNotificationsResponse result = _dataService.DeleteNotification(new DeleteNotificationsRequest
                                                                                     {
                                                                                         Id = notification.Id
                                                                                     });
            return result.Success;
        }
    }
}