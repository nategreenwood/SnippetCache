using System;
using System.Collections.Generic;
using System.Linq;
using SnippetCache.Data.EF4.Model;
using SnippetCache.Data.Wcf.Requests;
using SnippetCache.Data.Wcf.Responses;
using SnippetCache.Utils.Logging;
using SnippetCache.Utils.Validation;

namespace SnippetCache.Data.Wcf
{
    public partial class SnippetCacheDataService
    {
        #region NotificationType Operations

        public GetAllNotificationsResponse GetAllNotifications(GetAllNotificationsRequest request)
        {
            var response = new GetAllNotificationsResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                Notification[] results = _unitOfWork.NotificationRepository.Get(d => d.Id > 0).ToArray();
                Guard.ArgNotNull(results, "results");

                var quickInfoResults = new List<NotificationQuickInfo>(results.Count());
                quickInfoResults.AddRange(results.Select(
                    notification => new NotificationQuickInfo
                                        {
                                            Id = notification.Id,
                                            Text = notification.Text
                                        }));
                response.Success = true;
                response.Notifications = quickInfoResults.AsEnumerable();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetAllNotifications Method Failed", ex);
            }

            return response;
        }

        public GetAllNotificationsForUserResponse GetNotificationsForUser(GetAllNotificationsForUserRequest request)
        {
            var response = new GetAllNotificationsForUserResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                Notification[] results =
                    _unitOfWork.NotificationRepository.Get(
                        d => d.User_Id == request.UserId && d.User_FormsAuthId == request.UserGuid).ToArray();
                Guard.ArgNotNull(results, "results");

                var quickInfoResults = new List<NotificationQuickInfo>(results.Count());
                quickInfoResults.AddRange(results.Select(
                    notification => new NotificationQuickInfo
                                        {
                                            Id = notification.Id,
                                            Text = notification.Text,
                                            DateCreated = notification.DateCreated
                                        }));
                response.Success = true;
                response.Notifications = quickInfoResults.AsEnumerable();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetNotificationsForUser Method Failed", ex);
            }

            return response;
        }

        public GetNotificationsByIdResponse GetNotificationById(GetNotificationsByIdRequest request)
        {
            var response = new GetNotificationsByIdResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.NotificationRepository, "NotificationRepository");
                Notification notification = _unitOfWork.NotificationRepository.GetById(request.Id);

                Guard.ArgNotNull(notification, "notification");

                response.Id = notification.Id;
                response.Text = notification.Text;
                response.NotificationTypeId = notification.NotificationType_Id;
                response.UserId = notification.User_Id;
                response.UserFormsAuthId = notification.User_FormsAuthId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetNotificationById Method Failed", ex);
            }

            return response;
        }

        public GetNotificationsByNameResponse GetNotificationByName(GetNotificationsByNameRequest request)
        {
            // Do nothing
            return null;
        }

        // Create
        public CreateNewNotificationsResponse CreateNewNotification(CreateNewNotificationsRequest request)
        {
            var response = new CreateNewNotificationsResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.NotificationRepository, "_notificationRepo");

                var notification = new Notification
                                       {
                                           Text = request.Text,
                                           DateCreated = DateTime.UtcNow,
                                           NotificationType_Id = request.Notification_Type_Id,
                                           User_Id = request.User_Id,
                                           User_FormsAuthId = request.User_Forms_AuthId
                                       };

                _unitOfWork.NotificationRepository.Insert(notification);
                _unitOfWork.Save();

                int newId = notification.Id;
                if (newId > 0)
                {
                    response.Success = true;
                    response.Id = newId;
                    response.Text = notification.Text;
                    response.NotificationTypeId = notification.NotificationType_Id;
                    response.UserId = notification.User_Id;
                    response.UserFormsAuthId = notification.User_FormsAuthId;
                    Logger.LogInfo("Successfully created Notification Id: " + newId.ToString(), LogType.General);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateNewNotification Method Failed", ex);
            }

            return response;
        }

        // Update
        public UpdateNotificationsResponse UpdateNotification(UpdateNotificationsRequest request)
        {
            var response = new UpdateNotificationsResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                Notification notification = _unitOfWork.NotificationRepository.GetById(request.Id);

                Guard.ArgNotNull(notification, "notification");
                notification.Text = request.Text;
                notification.NotificationType_Id = request.Notification_Type_Id;
                notification.User_Id = request.User_Id;
                notification.User_FormsAuthId = request.User_Forms_AuthId;

                _unitOfWork.NotificationRepository.Update(notification);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully updated Notification Id: " + request.Id.ToString(), LogType.General);


                if (!response.Success)
                {
                    string errorString = "Unknown failure updating Notification Id: " + request.Id.ToString();
                    response.FailureInformation = errorString;
                    Logger.LogWarning(errorString, null);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("UpdateNotification method failed", ex);
            }

            return response;
        }

        // Delete
        public DeleteNotificationsResponse DeleteNotification(DeleteNotificationsRequest request)
        {
            var response = new DeleteNotificationsResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                Notification notification = _unitOfWork.NotificationRepository.GetById(request.Id);

                Guard.ArgNotNull(notification, "notification");

                _unitOfWork.NotificationRepository.Delete(notification);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully deleted Notification Id: " + request.Id.ToString(), LogType.General);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("DeleteNotification method failed", ex);
            }

            return response;
        }

        #endregion
    }
}