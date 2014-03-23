using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnippetCache.WebUI.Controllers;

namespace SnippetCache.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }
    }
}