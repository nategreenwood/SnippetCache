using System;
using System.Web;
using System.Web.Routing;

namespace SnippetCache.WebUI.Infrastructure
{
    public class GuidConstraint : IRouteConstraint
    {
        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            if (values.ContainsKey(parameterName))
            {
                var stringValue = values[parameterName] as string;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    Guid guidValue;

                    return Guid.TryParse(stringValue, out guidValue) && (guidValue != Guid.Empty);
                }
            }

            return false;
        }

        #endregion
    }
}