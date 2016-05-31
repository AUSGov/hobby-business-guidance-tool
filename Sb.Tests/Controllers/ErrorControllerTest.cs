using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sb.Web.Controllers;
using System.Web.Mvc;

namespace Sb.Tests.Controllers
{

    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ErrorControllerTest
    {
        [TestMethod]
        public void Error()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void BadRequest()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.BadRequest() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void NotFound()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.NotFound() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
