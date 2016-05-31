using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Entities.Models;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using Sb.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using Sb.Interfaces.Enums;

namespace Sb.Tests.Controllers
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ResultControllerTest
    {
        private ResultsController _controller;
        private Mock<IObligationManager> _fakeObligationManager;
        private Mock<IAnswerManager> _fakeAnswerManager;
        private Mock<IPersonaManager> _fakePersonaManager;
        private Mock<IQuestionManager> _fakeQuestionManager;
        private Mock<IHttpWrapper> _fakeHttpWrapper;

        [TestInitialize]
        public void Setup()
        {
            _fakeObligationManager = new Mock<IObligationManager>();
            _fakeAnswerManager = new Mock<IAnswerManager>();
            _fakePersonaManager = new Mock<IPersonaManager>();
            _fakeQuestionManager = new Mock<IQuestionManager>();
            _fakeHttpWrapper = new Mock<IHttpWrapper>();
            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(new Dictionary<IQuestion, IAnswer[]>());

            _controller = new ResultsController(_fakeObligationManager.Object, _fakeAnswerManager.Object, _fakePersonaManager.Object, _fakeQuestionManager.Object, _fakeHttpWrapper.Object);
        }


        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullObligationManager()
        {
            // Arrange
            _controller = new ResultsController(null, _fakeAnswerManager.Object, _fakePersonaManager.Object, _fakeQuestionManager.Object, _fakeHttpWrapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullAnswerManager()
        {
            // Arrange
            _controller = new ResultsController(_fakeObligationManager.Object, null, _fakePersonaManager.Object, _fakeQuestionManager.Object, _fakeHttpWrapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullPersonaManager()
        {
            // Arrange
            _controller = new ResultsController(_fakeObligationManager.Object, _fakeAnswerManager.Object, null, _fakeQuestionManager.Object, _fakeHttpWrapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullQuestionManager()
        {
            // Arrange
            _controller = new ResultsController(_fakeObligationManager.Object, _fakeAnswerManager.Object, _fakePersonaManager.Object, null, _fakeHttpWrapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullHttpWrapper()
        {
            // Arrange
            _controller = new ResultsController(_fakeObligationManager.Object, _fakeAnswerManager.Object, _fakePersonaManager.Object, _fakeQuestionManager.Object, null);
        }

        [TestMethod]
        public void Hobby_AnsweredAllQuestions()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "3" }, new IAnswer[] { new Answer() }}
            };

            var questions = new List<IQuestion>
            {
                new Question(),
                new Question(),
                new Question()
            };

            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakeQuestionManager.Setup(x => x.GetAllQuestions()).Returns(questions);
            _fakePersonaManager.Setup(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns(new Persona { OutcomeType = OutcomeType.Hobby});

           // Act
           var result = _controller.Hobby() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakePersonaManager.Verify(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>()), Times.Once());
            _fakeAnswerManager.Verify(x => x.GetAnswers(), Times.Once);
            _fakeQuestionManager.Verify(x => x.GetAllQuestions(), Times.Once);
            _fakeObligationManager.Verify(x => x.GetObligationsForVisitor(It.IsAny<IVisitor>()), Times.Once);
        }

        [TestMethod]
        public void Hobby_NotAnsweredAllQuestions()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }}
            };

            var questions = new List<IQuestion>
            {
                new Question(),
                new Question(),
                new Question()
            };

            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakeQuestionManager.Setup(x => x.GetAllQuestions()).Returns(questions);

            // Act
            var result = _controller.Hobby() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);

            _fakePersonaManager.Verify(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>()), Times.Never());
            _fakeAnswerManager.Verify(x => x.GetAnswers(), Times.Once);
            _fakeQuestionManager.Verify(x => x.GetAllQuestions(), Times.Once);
            _fakeObligationManager.Verify(x => x.GetObligationsForVisitor(It.IsAny<IVisitor>()), Times.Never);
        }

        [TestMethod]
        public void Business_AnsweredAllQuestions()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "3" }, new IAnswer[] { new Answer() }}
            };

            var questions = new List<IQuestion>
            {
                new Question(),
                new Question(),
                new Question()
            };

            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakeQuestionManager.Setup(x => x.GetAllQuestions()).Returns(questions);
            _fakePersonaManager.Setup(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns(new Persona { OutcomeType = OutcomeType.Business });

            // Act
            var result = _controller.Business() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakePersonaManager.Verify(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>()), Times.Once());
            _fakeAnswerManager.Verify(x => x.GetAnswers(), Times.Once);
            _fakeQuestionManager.Verify(x => x.GetAllQuestions(), Times.Once);
            _fakeObligationManager.Verify(x => x.GetObligationsForVisitor(It.IsAny<IVisitor>()), Times.Once);
        }

        [TestMethod]
        public void Unsure_AnsweredAllQuestions()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "3" }, new IAnswer[] { new Answer() }}
            };

            var questions = new List<IQuestion>
            {
                new Question(),
                new Question(),
                new Question()
            };

            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakeQuestionManager.Setup(x => x.GetAllQuestions()).Returns(questions);
            _fakePersonaManager.Setup(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns((Persona)null);

            // Act
            var result = _controller.Unsure() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakePersonaManager.Verify(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>()), Times.Once());
            _fakeAnswerManager.Verify(x => x.GetAnswers(), Times.Once);
            _fakeQuestionManager.Verify(x => x.GetAllQuestions(), Times.Once);
            _fakeObligationManager.Verify(x => x.GetObligationsForVisitor(It.IsAny<IVisitor>()), Times.Once);
        }

        [TestMethod]
        public void OpenHobbyShouldBeUnsure()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "3" }, new IAnswer[] { new Answer() }}
            };

            var questions = new List<IQuestion>
            {
                new Question(),
                new Question(),
                new Question()
            };

            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakeQuestionManager.Setup(x => x.GetAllQuestions()).Returns(questions);
            _fakePersonaManager.Setup(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns(new Persona { OutcomeType = OutcomeType.Unsure });
            _fakeHttpWrapper.Setup(x => x.QueryString).Returns(new NameValueCollection());

            // Act
            var result = _controller.Hobby();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Results", ((RedirectToRouteResult)result).RouteValues["controller"]);
            Assert.AreEqual("Unsure", ((RedirectToRouteResult)result).RouteValues["action"]);
        }

        [TestMethod]
        public void OpenBusinessShouldBeHobby()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "3" }, new IAnswer[] { new Answer() }}
            };

            var questions = new List<IQuestion>
            {
                new Question(),
                new Question(),
                new Question()
            };

            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakeQuestionManager.Setup(x => x.GetAllQuestions()).Returns(questions);
            _fakePersonaManager.Setup(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns(new Persona { OutcomeType = OutcomeType.Hobby });
            _fakeHttpWrapper.Setup(x => x.QueryString).Returns(new NameValueCollection());

            // Act
            var result = _controller.Business();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Results", ((RedirectToRouteResult)result).RouteValues["controller"]);
            Assert.AreEqual("Hobby", ((RedirectToRouteResult)result).RouteValues["action"]);
        }

        [TestMethod]
        public void OpenUnsureShouldBeBusiness()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "3" }, new IAnswer[] { new Answer() }}
            };

            var questions = new List<IQuestion>
            {
                new Question(),
                new Question(),
                new Question()
            };

            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakeQuestionManager.Setup(x => x.GetAllQuestions()).Returns(questions);
            _fakePersonaManager.Setup(x => x.GetPersona(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns(new Persona { OutcomeType = OutcomeType.Business });
            _fakeHttpWrapper.Setup(x => x.QueryString).Returns(new NameValueCollection());

            // Act
            var result = _controller.Unsure();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Results", ((RedirectToRouteResult)result).RouteValues["controller"]);
            Assert.AreEqual("Business", ((RedirectToRouteResult)result).RouteValues["action"]);
        }
    }
}