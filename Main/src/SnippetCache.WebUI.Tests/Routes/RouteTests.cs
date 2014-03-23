using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnippetCache.WebUI.Tests.Helpers;

namespace SnippetCache.WebUI.Tests.Routes
{
    [TestClass]
    public class RouteTests
    {
        [TestMethod]
        //[ExpectedException(typeof(AssertFailedException))]
        public void IncomingRoutesAllPassTest()
        {
            RouteHelpers.TestRouteMatch("~/", "Home", "Index");

            RouteHelpers.TestRouteMatch("~/Snippet/Action/56", "Snippet", "Action", new {id = "56"});
            RouteHelpers.TestRouteMatch("~/Snippet/List", "Snippet", "List");
            RouteHelpers.TestRouteMatch("~/Snippet", "Snippet", "Index");

            RouteHelpers.TestRouteMatch("~/Users", "User", "Index");
            RouteHelpers.TestRouteMatch("~/Users/List", "User", "List");
            RouteHelpers.TestRouteMatch("~/Users/Index/34", "User", "Index", new {id = "34"});

            RouteHelpers.TestRouteMatch("~/Admin/Index", "Admin", "Index");
            RouteHelpers.TestRouteMatch("~/One/Two", "One", "Two");
        }

        [TestMethod]
        public void IncomingRoutesCatchAllPassTest()
        {
            RouteHelpers.TestRouteMatch("~/Snippet/Action/Id/Segment/Too/Long", "Snippet", "Action",
                                        new {id = "Id", catchall = "Segment/Too/Long"});
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void IncomingRoutesCatchAllFailTest()
        {
            RouteHelpers.TestRouteMatch("~/Snippet/List/34/Segment/Too/Long", "Snippet", "List",
                                        new {id = "34", catchall = "Segment/Too/Lg"});
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void IncomingRoutesTooManySegmentsFailTest()
        {
            // Ensure too many segments fail
            RouteHelpers.TestRouteFail("~/Admin/Index/Segment");
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void IncomingRoutesTooFewSegmentsFailTest()
        {
            RouteHelpers.TestRouteFail("~/Admin");
        }

        [TestMethod]
        public void OutgoingRoutesPassTest()
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var requestContext = new RequestContext(RouteHelpers.CreateHttpContext(), new RouteData());

            string result = UrlHelper.GenerateUrl(null, "Index", "Home", null, routes, requestContext, true);
            Assert.AreEqual("/", result);
        }
    }
}