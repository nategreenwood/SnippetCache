using System;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SnippetCache.WebUI.Tests.Helpers
{
    public class RouteHelpers
    {
        public static void TestRouteFail(string url)
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            RouteData result = routes.GetRouteData(CreateHttpContext(url));

            Assert.IsTrue(result == null || result.Route == null);
        }

        public static bool TestIncomingRouteResult(RouteData routeResult, string controller, string action,
                                                   object propertySet = null)
        {
            Func<object, object, bool> valCompare =
                (v1, v2) => StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;

            bool result = valCompare(routeResult.Values["controller"], controller) &&
                          valCompare(routeResult.Values["action"], action);

            if (propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in propInfo)
                {
                    if (!(routeResult.Values.ContainsKey(propertyInfo.Name) &&
                          valCompare(routeResult.Values[propertyInfo.Name], propertyInfo.GetValue(propertySet, null))))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        public static RouteData TestRouteMatch(string url, string controller, string action,
                                               object routeProperties = null,
                                               string httpMethod = "GET")
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));

            return result;
        }

        public static HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(d => d.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(d => d.HttpMethod).Returns(httpMethod);

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(d => d.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(d => d.Request).Returns(mockRequest.Object);
            mockContext.Setup(d => d.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }
    }
}