using System;
using System.Security.Principal;
using log4net;

namespace SnippetCache.Utils.Logging
{
    public static class Logger
    {
        private const string DEFAULT_APPLICATION_LOGGER = "Application";

        public static void LogError(string message, Exception @error)
        {
            ILog logger = LogManager.GetLogger(DEFAULT_APPLICATION_LOGGER);
            if (@error.InnerException != null)
                @error = @error.InnerException;

            if (logger.IsErrorEnabled)
                logger.Error(message, @error);
        }

        public static void LogError(string message, IPrincipal user, Uri url, Exception @error)
        {
            SetOptionalParameters(user, url);
            LogError(message, @error);
        }

        public static void LogInfo(string message, LogType type)
        {
            ILog logger = null;

            if (type == LogType.Notify)
            {
                logger = LogManager.GetLogger(LogType.Notify.ToString());
            }
            else
            {
                logger = LogManager.GetLogger(DEFAULT_APPLICATION_LOGGER);
            }
            if (logger.IsInfoEnabled)
                logger.Info(message);
        }

        public static void LogWarning(string message, Exception @error)
        {
            ILog logger = LogManager.GetLogger(DEFAULT_APPLICATION_LOGGER);

            if (@error.InnerException != null)
                @error = @error.InnerException;

            if (logger.IsWarnEnabled)
                logger.Warn(message, @error);
        }

        public static void LogWarning(string message, IPrincipal user, Uri url, Exception @error)
        {
            SetOptionalParameters(user, url);
            LogWarning(message, @error);
        }

        private static void SetOptionalParameters(IPrincipal user, Uri url)
        {
            if (user != null && user.Identity.IsAuthenticated)
            {
                MDC.Set("user", user.Identity.Name);
            }

            MDC.Set("url", url.ToString());
        }
    }

    public enum LogType
    {
        General,
        Notify
    }
}