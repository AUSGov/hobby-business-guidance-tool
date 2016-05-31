using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sb.Web.Controllers;
using System.Web.Mvc;
using Moq;
using Sb.Entities.Models;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using Sb.Web.Models;

namespace Sb.Tests.Controllers
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HomeControllerTest
    {
        private Mock<IQuestionManager> _fakeQuestionManager;
        private Mock<IAnswerManager> _fakeAnswerManager;
        private Mock<IObligationManager> _fakeObligationManager;
        private Mock<IPersonaManager> _fakePersonaManager;
        private HomeController _controller;


        [TestInitialize]
        public void Setup()
        {
            _fakeQuestionManager = new Mock<IQuestionManager>();
            _fakeAnswerManager = new Mock<IAnswerManager>();
            _fakePersonaManager = new Mock<IPersonaManager>();
            _fakeObligationManager = new Mock<IObligationManager>();

            _controller = new HomeController(_fakeQuestionManager.Object, _fakeAnswerManager.Object, _fakeObligationManager.Object, _fakePersonaManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullQuestionManager()
        {
            // Assert
            _controller = new HomeController(null, _fakeAnswerManager.Object, _fakeObligationManager.Object, _fakePersonaManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullAnswerManager()
        {
            // Assert
            _controller = new HomeController(_fakeQuestionManager.Object, null, _fakeObligationManager.Object, _fakePersonaManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullObligationManager()
        {
            // Assert
            _controller = new HomeController(_fakeQuestionManager.Object, _fakeAnswerManager.Object, null, _fakePersonaManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullPersonaManager()
        {
            // Assert
            _controller = new HomeController(_fakeQuestionManager.Object, _fakeAnswerManager.Object, _fakeObligationManager.Object, null);
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            _fakeQuestionManager.Setup(x => x.GetNextQuestion(null)).Returns(new Question { Id = "100" });

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("100", ((LandingModel)result.Model).FirstQuestionId);
            _fakeQuestionManager.Verify(x => x.GetNextQuestion(null), Times.Once);
        }

        [TestMethod]
        public void Warmup()
        {
            // Arrange
            _fakeQuestionManager.Setup(x => x.GetAllQuestions()).Returns(new List<IQuestion>());
            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(new Dictionary<IQuestion, IAnswer[]>());
            _fakeObligationManager.Setup(x => x.GetAllObligations()).Returns(new List<IObligation>());
            _fakePersonaManager.Setup(x => x.GetAllPersonas()).Returns(new List<IPersona>());

            // Act
            var result = _controller.Warmup() as JsonResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
