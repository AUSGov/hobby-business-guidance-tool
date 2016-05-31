using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Entities;
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
    public class QuestionControllerTest
    {
        private Mock<IQuestionManager> _fakeQuestionManager;
        private Mock<IAnswerManager> _fakeAnswerManager;
        private Mock<IHttpWrapper> _fakeHttpWrapper;
        private Mock<IPersonaManager> _fakePersonaManager;
        private QuestionsController _controller;


        [TestInitialize]
        public void Setup()
        {
            _fakeQuestionManager = new Mock<IQuestionManager>();
            _fakeAnswerManager = new Mock<IAnswerManager>();
            _fakePersonaManager = new Mock<IPersonaManager>();
            _fakeHttpWrapper = new Mock<IHttpWrapper>();

            _controller = new QuestionsController(_fakeQuestionManager.Object, _fakeAnswerManager.Object, _fakePersonaManager.Object, _fakeHttpWrapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullQuestionManager()
        {
            // Assert
            _controller = new QuestionsController(null, _fakeAnswerManager.Object, _fakePersonaManager.Object, _fakeHttpWrapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullAnswerManager()
        {
            // Assert
            _controller = new QuestionsController(_fakeQuestionManager.Object, null, _fakePersonaManager.Object, _fakeHttpWrapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullPersonaManager()
        {
            // Assert
            _controller = new QuestionsController(_fakeQuestionManager.Object, _fakeAnswerManager.Object, null, _fakeHttpWrapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullHttpWrapper()
        {
            // Assert
            _controller = new QuestionsController(_fakeQuestionManager.Object, _fakeAnswerManager.Object, _fakePersonaManager.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullId()
        {
            // Assert
            _controller.Index(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsErrorOnPassingInInvalidId()
        {
            // Arrange
            _fakeQuestionManager.Setup(x => x.GetQuestion("10")).Returns((Question)null);

            // Assert
            _controller.Index("10");
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
            };

            var question = new Question { Id = "1", QuestionNumber = 3};
            _fakeQuestionManager.Setup(x => x.GetQuestion("1")).Returns(question);
            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);

            // Act
            var result = _controller.Index("1") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeQuestionManager.Verify(x => x.GetQuestion("1"), Times.Once);
            _fakeAnswerManager.Verify(x => x.RetrieveAnswerId("1"), Times.Once);
        }

        [TestMethod]
        public void IndexWithId()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
            };

            var question = new Question { Id = "100", QuestionNumber = 3 };
            _fakeQuestionManager.Setup(x => x.GetQuestion("100")).Returns(question);
            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);

            // Act
            var result = _controller.Index("100") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeQuestionManager.Verify(x => x.GetNextQuestion(null), Times.Never);
            _fakeAnswerManager.Verify(x => x.RetrieveAnswerId("100"), Times.Once);
            _fakeQuestionManager.Verify(x => x.GetQuestion(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Index_AnsweredMoreQuestions()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "2" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "3" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "4" }, new IAnswer[] { new Answer() }},
                {new Question { Id = "5" }, new IAnswer[] { new Answer() }}
            };

            var question = new Question { Id = "1", QuestionNumber = 3 };
            _fakeQuestionManager.Setup(x => x.GetQuestion("1")).Returns(question);
            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);

            // Act
            var result = _controller.Index("1") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _fakeQuestionManager.Verify(x => x.GetQuestion("1"), Times.Once);
            _fakeAnswerManager.Verify(x => x.RetrieveAnswerId("1"), Times.Once);
        }

        [TestMethod]
        public void Index_AnsweredNotEnoughQuestions()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question { Id = "1" }, new IAnswer[] { new Answer() }},
            };

            var question = new Question { Id = "1", QuestionNumber = 3 };
            _fakeQuestionManager.Setup(x => x.GetQuestion("1")).Returns(question);
            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakePersonaManager.Setup(x => x.GetOutcomeType(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns(OutcomeType.Hobby);

            // Act
            var result = _controller.Index("1") as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            _fakeQuestionManager.Verify(x => x.GetQuestion("1"), Times.Once);
            _fakeAnswerManager.Verify(x => x.RetrieveAnswerId("1"), Times.Never);
        }

        [TestMethod]
        public void MoveToPrevious()
        {
            // Arrange
            var currentQuestion = new Question {Id = "2"};
            var previousQuestion = new Question {Id = "1"};

            _fakeQuestionManager.Setup(x => x.GetPreviousQuestion(currentQuestion.Id)).Returns(previousQuestion);
            _fakeHttpWrapper.Setup(x => x.Form).Returns(new NameValueCollection());

            // Act
            var result = _controller.Index(currentQuestion, Constants.SubmitType.Previous);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues["Action"]);
            Assert.AreEqual("1", ((RedirectToRouteResult)result).RouteValues["Id"]);

            _fakeQuestionManager.Verify(x => x.GetPreviousQuestion(currentQuestion.Id), Times.Once);
            _fakeAnswerManager.Verify(x => x.RetrieveAnswerId(previousQuestion.Id), Times.Never);
            _fakeAnswerManager.Verify(x => x.StoreAnswerId(currentQuestion.Id, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void MoveToPreviousNoPrevious()
        {
            // Arrange
            var currentQuestion = new Question {Id = "2"};
            _fakeQuestionManager.Setup(x => x.GetPreviousQuestion(currentQuestion.Id)).Returns((Question) null);
            _fakeHttpWrapper.Setup(x => x.Form).Returns(new NameValueCollection());

            // Act
            var result = _controller.Index(currentQuestion, Constants.SubmitType.Previous);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Home", ((RedirectToRouteResult)result).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult) result).RouteValues["action"]);
            _fakeQuestionManager.Verify(x => x.GetPreviousQuestion(currentQuestion.Id), Times.Once);
            _fakeAnswerManager.Verify(x => x.StoreAnswerId(currentQuestion.Id, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void MoveToNext()
        {
            // Arrange
            var currentQuestion = new Question { Id = "1" };
            var nextQuestion = new Question { Id = "2" };
            _fakeQuestionManager.Setup(x => x.GetNextQuestion(currentQuestion.Id)).Returns(nextQuestion);
            _fakeHttpWrapper.Setup(x => x.Form).Returns(new NameValueCollection());

            // Act
            var result = _controller.Index(currentQuestion, Constants.SubmitType.Next);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues["Action"]);
            Assert.AreEqual("2", ((RedirectToRouteResult)result).RouteValues["Id"]);

            _fakeQuestionManager.Verify(x => x.GetNextQuestion(currentQuestion.Id), Times.Once);
            _fakeAnswerManager.Verify(x => x.RetrieveAnswerId(nextQuestion.Id), Times.Never);
            _fakeAnswerManager.Verify(x => x.StoreAnswerId(currentQuestion.Id, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void MoveToNextNoNext()
        {
            // Arrange
            var answers = new Dictionary<IQuestion, IAnswer[]>();
            var currentQuestion = new Question { Id = "2" };
            _fakeQuestionManager.Setup(x => x.GetNextQuestion(currentQuestion.Id)).Returns((Question)null);
            _fakeHttpWrapper.Setup(x => x.Form).Returns(new NameValueCollection());
            _fakeAnswerManager.Setup(x => x.GetNameValueCollection()).Returns(new NameValueCollection {{"freckle", "100"}});
            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakePersonaManager.Setup(x => x.GetOutcomeType(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns(OutcomeType.Hobby);

            // Act
            var result = _controller.Index(currentQuestion, Constants.SubmitType.Next) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Results", result.RouteValues["controller"]);
            Assert.AreEqual("Hobby", result.RouteValues["action"]);
            Assert.AreEqual("100", result.RouteValues["freckle"]);
            _fakeQuestionManager.Verify(x => x.GetNextQuestion(currentQuestion.Id), Times.Once);
            _fakeAnswerManager.Verify(x => x.StoreAnswerId(currentQuestion.Id, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void MoveToResultsAsBusiness()
        {
            MoveToResultsAsOutcomeType(OutcomeType.Business);
        }

        [TestMethod]
        public void MoveToResultsAsHobby()
        {
            MoveToResultsAsOutcomeType(OutcomeType.Hobby);
        }

        [TestMethod]
        public void MoveToResultsAsUnsure()
        {
            MoveToResultsAsOutcomeType(OutcomeType.Unsure);
        }

        private void MoveToResultsAsOutcomeType(OutcomeType outcomeType)
        {
            var answers = new Dictionary<IQuestion, IAnswer[]>
            {
                {new Question {Id = "1"}, new IAnswer[] {new Answer(), new Answer()}}
            };

            var currentQuestion = new Question {Id = "2"};
            _fakeQuestionManager.Setup(x => x.GetNextQuestion(currentQuestion.Id)).Returns((Question) null);
            _fakeHttpWrapper.Setup(x => x.Form).Returns(new NameValueCollection());
            _fakeAnswerManager.Setup(x => x.GetNameValueCollection()).Returns(new NameValueCollection {{"freckle", "100"}});
            _fakeAnswerManager.Setup(x => x.GetAnswers()).Returns(answers);
            _fakePersonaManager.Setup(x => x.GetOutcomeType(It.IsAny<IDictionary<string, IAnswer[]>>())).Returns(outcomeType);

            // Act
            var result = _controller.Index(currentQuestion, Constants.SubmitType.Results) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (RedirectToRouteResult));
            Assert.AreEqual("Results", result.RouteValues["controller"]);
            Assert.AreEqual(outcomeType.ToString(), result.RouteValues["action"]);
            Assert.AreEqual("100", result.RouteValues["freckle"]);
            _fakeQuestionManager.Verify(x => x.GetNextQuestion(currentQuestion.Id), Times.Never);
            _fakeAnswerManager.Verify(x => x.StoreAnswerId(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}