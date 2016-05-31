using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Entities.Models;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using Sb.Services.Managers;
using System;
using System.Collections.Generic;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class QuestionManagerTest
    {
        private Mock<IQuestionLoader> _fakeQuestionLoader;
        private QuestionManager _manager;

        [TestInitialize]
        public void Setup()
        {
            _fakeQuestionLoader = new Mock<IQuestionLoader>();
            _fakeQuestionLoader.Setup(x => x.GetQuestions()).Returns(GetQuestions());

            _manager = new QuestionManager(_fakeQuestionLoader.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullObject()
        {
            // Act
            _manager = new QuestionManager(null);
        }

        [TestMethod]
        public void ReturnsFirstQuestionWhenPassingInNull()
        {
            // Act
            var question = _manager.GetNextQuestion();

            // Assert
            Assert.IsNotNull(question);
            Assert.AreEqual("1", question.Id);
        }

        [TestMethod]
        public void ReturnsNullWhenNoQuestions()
        {
            // Arrange
            _fakeQuestionLoader.Setup(x => x.GetQuestions()).Returns(new List<IQuestion>());

            // Act
            var question = _manager.GetNextQuestion();

            // Assert
            Assert.IsNull(question);
        }


        [TestMethod]
        public void ReturnsNextWhenAsked()
        {
            // Act
            var question = _manager.GetNextQuestion("1");

            // Assert
            Assert.IsNotNull(question);
            Assert.AreEqual("2", question.Id);
        }

        [TestMethod]
        public void ReturnsNullWhenAskedForNextAtEnd()
        {
            // Act
            var question = _manager.GetNextQuestion("5");

            // Assert
            Assert.IsNull(question);
        }

        [TestMethod]
        public void ReturnsPreviousWhenAsked()
        {
            // Act
            var question = _manager.GetPreviousQuestion("4");

            // Assert
            Assert.IsNotNull(question);
            Assert.AreEqual("3", question.Id);
        }

        [TestMethod]
        public void ReturnsNullWhenAskedForPreviousAtStart()
        {
            // Act
            var question = _manager.GetPreviousQuestion("1");

            // Assert
            Assert.IsNull(question);
        }

        [TestMethod]
        public void GetQuestion_Found()
        {
            // Act
            var question = _manager.GetQuestion("1");

            // Assert
            Assert.IsNotNull(question);
            Assert.AreEqual("1", question.Id);
        }

        [TestMethod]
        public void GetQuestion_NotFound()
        {
            // Act
            var question = _manager.GetQuestion("XXX");

            // Assert
            Assert.IsNull(question);
        }

        [TestMethod]
        public void GetAllQuestions()
        {
            // Act
            var questions = _manager.GetAllQuestions();

            // Assert
            Assert.IsNotNull(questions);
            Assert.AreEqual(5, questions.Count);
        }

        private static List<IQuestion> GetQuestions()
        {
            var result = new List<IQuestion>
            {
                new Question {Id = "1"},
                new Question {Id = "2"},
                new Question {Id = "3"},
                new Question {Id = "4"},
                new Question {Id = "5"}
            };

            return result;
        }
    }
}
