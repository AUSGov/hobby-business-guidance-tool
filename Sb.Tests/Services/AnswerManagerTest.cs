using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Entities;
using Sb.Entities.Models;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using Sb.Services.Managers;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AnswerManagerTest
    {
        private Mock<ICookieContext> _fakeCookieContext;
        private Mock<IQuestionManager> _fakeQuestionManager;
        private IAnswerManager _manager;


        [TestInitialize]
        public void Setup()
        {
            _fakeCookieContext = new Mock<ICookieContext>();
            _fakeQuestionManager = new Mock<IQuestionManager>();

            _manager = new AnswerManager(_fakeCookieContext.Object, _fakeQuestionManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullCookieContext()
        {
            // Act
            _manager = new AnswerManager(null, _fakeQuestionManager.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullQuestionManager()
        {
            // Act
            _manager = new AnswerManager(_fakeCookieContext.Object, null);
        }

        [TestMethod]
        public void RetrieveAnswerId_Exists()
        {
            // Arrange
            var cookie = new HttpCookie(Constants.CookieName.Answers) {["1"] = "aaa"};
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns(cookie);

            // Act
            var answerId = _manager.RetrieveAnswerId("1");

            // Assert
            Assert.AreEqual("aaa", answerId);
        }

        [TestMethod]
        public void RetrieveAnswerId_NotExists()
        {
            // Act
            var answerId = _manager.RetrieveAnswerId("2");

            // Assert
            Assert.IsNull(answerId);
        }

        [TestMethod]
        public void StoreAnswerId_CookieExists()
        {
            // Arrange
            var cookie = new HttpCookie(Constants.CookieName.Answers) { ["1"] = "aaa" };
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns(cookie);
            _fakeQuestionManager.Setup(x => x.GetQuestion("2")).Returns(new Question { AnswerList = new IAnswer[] { new Answer { Id = "aaa"}, new Answer { Id = "bbb" } } });

            // Act
            _manager.StoreAnswerId("2", "bbb");

            // Assert
            _fakeCookieContext.Verify(x => x.SetCookie(cookie), Times.Once);
        }

        [TestMethod]
        public void StoreAnswerId_NewCookie()
        {
            // Arrange
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns((HttpCookie) null);
            _fakeQuestionManager.Setup(x => x.GetQuestion("2")).Returns(new Question { AnswerList = new IAnswer[] { new Answer { Id = "aaa" }, new Answer { Id = "bbb" } } });

            // Act
            _manager.StoreAnswerId("2", "bbb");

            // Assert
            _fakeCookieContext.Verify(x => x.SetCookie(It.IsAny<HttpCookie>()), Times.Once);
        }

        [TestMethod]
        public void StoreAnswerId_InvalidQuestion()
        {
            // Arrange
            var cookie = new HttpCookie(Constants.CookieName.Answers) { ["1"] = "aaa" };
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns(cookie);
            _fakeQuestionManager.Setup(x => x.GetQuestion("2")).Returns((Question)null);

            // Act
            _manager.StoreAnswerId("99", "hhh");

            // Assert
            _fakeCookieContext.Verify(x => x.SetCookie(cookie), Times.Never);
        }

        [TestMethod]
        public void StoreAnswerId_InvalidAnswer()
        {
            // Arrange
            var cookie = new HttpCookie(Constants.CookieName.Answers) { ["1"] = "aaa" };
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns(cookie);
            _fakeQuestionManager.Setup(x => x.GetQuestion("2")).Returns(new Question { AnswerList = new IAnswer[] { new Answer { Id = "aaa" }, new Answer { Id = "bbb" } } });

            // Act
            _manager.StoreAnswerId("2", "zzz");

            // Assert
            _fakeCookieContext.Verify(x => x.SetCookie(cookie), Times.Once);
        }

        [TestMethod]
        public void StoreAnswers()
        {
            // Arrange
            var nameValueCollection = new NameValueCollection {{"aaa", "111"}, {"bbb", "222"}, {"ccc", "333"}};

            var cookie = new HttpCookie(Constants.CookieName.Answers) { ["1"] = "aaa" };
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns(cookie);
            _fakeQuestionManager.Setup(x => x.GetQuestion("aaa")).Returns(new Question { AnswerList = new IAnswer[] { new Answer { Id = "111" }, new Answer { Id = "123" } } });
            _fakeQuestionManager.Setup(x => x.GetQuestion("bbb")).Returns(new Question { AnswerList = new IAnswer[] { new Answer { Id = "222" }, new Answer { Id = "123" } } });
            _fakeQuestionManager.Setup(x => x.GetQuestion("ccc")).Returns(new Question { AnswerList = new IAnswer[] { new Answer { Id = "333" }, new Answer { Id = "123" } } });

            // Act
            _manager.StoreAnswers(nameValueCollection);

            // Assert
            _fakeCookieContext.Verify(x => x.SetCookie(cookie), Times.Exactly(3));
        }

        [TestMethod]
        public void GetAnswers_NoCookie()
        {
            // Arrange
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns((HttpCookie) null);

            // Act
            var result = _manager.GetAnswers();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAnswers_CookieExists_NoQuestions()
        {
            // Arrange
            var cookie = new HttpCookie(Constants.CookieName.Answers)
            {
                ["1"] = "aaa",
                ["2"] = "bbb",
                ["3"] = "ccc"
            };

            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns(cookie);

            // Act
            var result = _manager.GetAnswers();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAnswers_CookieExists_QuestionsExist()
        {
            // Arrange
            var cookie = new HttpCookie(Constants.CookieName.Answers)
            {
                ["1"] = "aaa",
                ["2"] = "bbb",
                ["3"] = "ccc"
            };

            _fakeQuestionManager.Setup(x => x.GetQuestion("1")).Returns(new Question { AnswerList = new IAnswer[] {} });
            _fakeQuestionManager.Setup(x => x.GetQuestion("2")).Returns(new Question { AnswerList = new IAnswer[] {} });
            _fakeQuestionManager.Setup(x => x.GetQuestion("3")).Returns(new Question { AnswerList = new IAnswer[] { new Answer { Id = "ccc" } } });
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns(cookie);

            // Act
            var result = _manager.GetAnswers();

            //Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void GetAnswers_CookieExists_QuestionsExist_AnswersNotExist()
        {
            // Arrange
            var cookie = new HttpCookie(Constants.CookieName.Answers)
            {
                ["1"] = "",
                ["2"] = "freckle,smith",
                ["3"] = null
            };

            _fakeQuestionManager.Setup(x => x.GetQuestion("1")).Returns(new Question { AnswerList = new IAnswer[] { } });
            _fakeQuestionManager.Setup(x => x.GetQuestion("2")).Returns(new Question { AnswerList = new IAnswer[] { } });
            _fakeQuestionManager.Setup(x => x.GetQuestion("3")).Returns(new Question { AnswerList = new IAnswer[] { new Answer { Id = "ccc" } } });
            _fakeCookieContext.Setup(x => x.GetCookie(Constants.CookieName.Answers)).Returns(cookie);

            // Act
            var result = _manager.GetAnswers();

            //Assert
            Assert.AreEqual(3, result.Count);
        }
    }
}

