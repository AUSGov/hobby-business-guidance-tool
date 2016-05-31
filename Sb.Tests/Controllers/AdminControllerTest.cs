using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Interfaces.Services;
using Sb.Web.Controllers;
using System;
using System.Web.Mvc;

namespace Sb.Tests.Controllers
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AdminControllerTest
    {
        private AdminController _controller;
        private Mock<IQuestionManager> _fakeQuestionManager;
        private Mock<IObligationManager> _fakeObligationManager;
        private Mock<IPersonaManager> _fakePersonaManager;

        [TestInitialize]
        public void Setup()
        {
            _fakeQuestionManager = new Mock<IQuestionManager>();
            _fakeObligationManager = new Mock<IObligationManager>();
            _fakePersonaManager = new Mock<IPersonaManager>();

            _controller = new AdminController(_fakeQuestionManager.Object, _fakeObligationManager.Object, _fakePersonaManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullQuestionManager()
        {
            // Assert
            _controller = new AdminController(null, _fakeObligationManager.Object, _fakePersonaManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullObligationManager()
        {
            // Assert
            _controller = new AdminController(_fakeQuestionManager.Object, null, _fakePersonaManager.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullPersonaManager()
        {
            // Assert
            _controller = new AdminController(_fakeQuestionManager.Object, _fakeObligationManager.Object, null);
        }


        [TestMethod]
        public void QuestionSummary()
        {
            // Act
            var result = _controller.QuestionSummary() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeQuestionManager.Verify(x => x.GetAllQuestions(), Times.Once);
        }

        [TestMethod]
        public void QuestionDetail()
        {
            // Act
            var result = _controller.QuestionDetail() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeQuestionManager.Verify(x => x.GetAllQuestions(), Times.Once);
        }

        [TestMethod]
        public void ObligationDetail()
        {
            // Act
            var result = _controller.ObligationDetail() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeObligationManager.Verify(x => x.GetAllObligations(), Times.Once);
        }

        [TestMethod]
        public void AnswerDetail()
        {
            // Act
            var result = _controller.AnswerDetail() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeQuestionManager.Verify(x => x.GetAllQuestions(), Times.Once);
        }

        [TestMethod]
        public void PersonaDetail()
        {
            // Act
            var result = _controller.PersonaDetail() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeQuestionManager.Verify(x => x.GetAllQuestions(), Times.Once);
            _fakePersonaManager.Verify(x => x.GetAllPersonas(), Times.Once);
        }
    }
}
