using System.Web.Mvc;

namespace SnippetCache.Utils.Logging
{
    public class LogAllAttribute : FilterAttribute, IActionFilter
    {
        #region IActionFilter Members

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Logger.LogInfo(string.Format("====== Action Executed Controller: {0} Action: {1}",
                                         filterContext.RouteData.Values["controller"],
                                         filterContext.RouteData.Values["action"]), LogType.General);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Logger.LogInfo(string.Format("Action Executing Controller: {0} Action: {1}",
                                         filterContext.Controller, filterContext.ActionDescriptor), LogType.General);
        }

        #endregion
    }
}