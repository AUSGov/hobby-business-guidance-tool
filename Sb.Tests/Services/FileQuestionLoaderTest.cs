using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using Sb.Services.Loaders;
using Sb.Services.Managers;
using System;
using System.Collections.Generic;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class FileQuestionLoaderTest
    {
        private Mock<IFilePathResolver> _fakeFilePathResolver;
        private Mock<ICacheManager> _fakeCacheManger;
        private ICacheManager _realCacheManger;
        private IQuestionLoader _loader;

        private const string DirectoryPath = "..\\..\\_Data";
        private const string DirectoryName = "Questions";
        private const string CacheName = "FileQuestionLoaderTest";

        [TestInitialize]
        public void Setup()
        {
            _fakeFilePathResolver = new Mock<IFilePathResolver>();
            _fakeCacheManger = new Mock<ICacheManager>();
            _realCacheManger = new CacheManager();
            _fakeFilePathResolver.Setup(x => x.GetWorkingDirectory()).Returns(DirectoryPath);

            _loader = new FileQuestionLoader(_realCacheManger, _fakeFilePathResolver.Object, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullCacheManager()
        {
            // Act
            _loader = new FileQuestionLoader(null, _fakeFilePathResolver.Object, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullFilePathResolver()
        {
            // Act
            _loader = new FileQuestionLoader(_fakeCacheManger.Object, null, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullCacheName()
        {
            // Act
            _loader = new FileQuestionLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName, string.Empty);
        }

        [TestMethod]
        public void LoadQuestions()
        {
            // Act
            var questions = _loader.GetQuestions();

            // Assert
            Assert.IsNotNull(questions);
            Assert.AreEqual(4, questions.Count);
            Assert.AreEqual(1, questions[0].QuestionNumber);
            Assert.AreEqual(2, questions[1].QuestionNumber);
            Assert.AreEqual(3, questions[2].QuestionNumber);
            Assert.AreEqual(4, questions[3].QuestionNumber);
            Assert.AreEqual(4, questions[0].QuestionCount);
            Assert.AreEqual(4, questions[1].QuestionCount);
            Assert.AreEqual(4, questions[2].QuestionCount);
            Assert.AreEqual(4, questions[3].QuestionCount);
        }

        [TestMethod]
        public void LoadsHtmlValuesFromCorrectly()
        {
            // Act
            var questions = _loader.GetQuestions();

            // Assert
            Assert.IsNotNull(questions);
            // no file, use deserialized value
            Assert.AreEqual("Q1-Description", questions[0].Description);
            Assert.AreEqual("Q1-MoreInfo", questions[0].MoreInfo);
            // load from file when empty
            Assert.AreEqual("Q2-DESC-FROM-HTML", questions[1].Description);
            Assert.AreEqual("Q2-MOREINFO-FROM-HTML", questions[1].MoreInfo);
            // use deserialized value when it exists
            Assert.AreEqual("Q3-Description", questions[2].Description);
            Assert.AreEqual("Q3-MoreInfo", questions[2].MoreInfo);
        }

        [TestMethod]
        public void LoadsFromCache()
        {
            // Arrange
            _loader = new FileQuestionLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName, CacheName);
            _fakeCacheManger.Setup(x => x.Contains(CacheName)).Returns(true);

            // Act
            _loader.GetQuestions();

            //Assert
            _fakeCacheManger.Verify(x => x.Get(CacheName), Times.Once);
            _fakeCacheManger.Verify(x => x.Add(CacheName, It.IsAny<List<IQuestion>>()), Times.Never);
        }

        [TestMethod]
        public void AddsToCache()
        {
            // Arrange
            _loader = new FileQuestionLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName, CacheName);
            _fakeCacheManger.Setup(x => x.Contains(CacheName)).Returns(false);

            // Act
            _loader.GetQuestions();

            //Assert
            _fakeCacheManger.Verify(x => x.Get(CacheName), Times.Once);
            _fakeCacheManger.Verify(x => x.Add(CacheName, It.IsAny<List<IQuestion>>()), Times.Once);
        }
    }
}
