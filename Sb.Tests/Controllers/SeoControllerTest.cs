using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Entities;
using Sb.Interfaces;
using Sb.Web.Controllers;
using System;
using System.Web.Mvc;

namespace Sb.Tests.Controllers
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SeoControllerTest
    {
        private SeoController _controller;
        private Mock<ISettingsReader> _fakeSettingsReader;

        [TestInitialize]
        public void Setup()
        {
            _fakeSettingsReader = new Mock<ISettingsReader>();

            _controller = new SeoController(_fakeSettingsReader.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullSettingsReader()
        {
            // Assert
            _controller = new SeoController(null);
        }

        [TestMethod]
        public void RobotsText()
        {
            // Arrange
            _fakeSettingsReader.Setup(x => x[Constants.AppSettings.RobotsFile]).Returns("robots.txt");

            // Act
            var result = _controller.Robots() as FilePathResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeSettingsReader.Verify(x => x[Constants.AppSettings.RobotsFile], Times.Once);
        }
    }
}
