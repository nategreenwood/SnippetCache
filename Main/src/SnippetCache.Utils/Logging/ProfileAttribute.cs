using System.Diagnostics;
using System.Web.Mvc;

namespace SnippetCache.Utils.Logging
{
    public class ProfileAttribute : FilterAttribute, IActionFilter
    {
        private Stopwatch _timer;

        #region IActionFilter Members

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _timer.Stop();
            if (filterContext.Exception == null)
            {
                filterContext.HttpContext.Response.Write(string.Format("Action method elapsed time: {0:d}",
                                                                       _timer.ElapsedMilliseconds));
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _timer = Stopwatch.StartNew();
        }

        #endregion
    }
}