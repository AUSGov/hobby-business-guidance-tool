using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using Sb.Services.Loaders;
using Sb.Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class FilePersonaLoaderTest
    {
        private Mock<IFilePathResolver> _fakeFilePathResolver;
        private Mock<ICacheManager> _fakeCacheManger;
        private ICacheManager _realCacheManger;
        private IPersonaLoader _loader;

        private const string DirectoryPath = "..\\..\\_Data";
        private const string DirectoryName = "Personas";
        private const string CacheName = "FilePersonaLoaderTest";

        [TestInitialize]
        public void Setup()
        {
            _fakeFilePathResolver = new Mock<IFilePathResolver>();
            _fakeCacheManger = new Mock<ICacheManager>();
            _realCacheManger = new CacheManager();
            _fakeFilePathResolver.Setup(x => x.GetWorkingDirectory()).Returns(DirectoryPath);

            _loader = new FilePersonaLoader(_realCacheManger, _fakeFilePathResolver.Object, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullCacheManager()
        {
            // Act
            _loader = new FilePersonaLoader(null, _fakeFilePathResolver.Object, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullFilePathResolver()
        {
            // Act
            _loader = new FilePersonaLoader(_fakeCacheManger.Object, null, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullCacheName()
        {
            // Act
            _loader = new FilePersonaLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName, string.Empty);
        }

        [TestMethod]
        public void LoadPersonas()
        {
            // Act
            var questions = _loader.GetPersonas();

            // Assert
            Assert.IsNotNull(questions);
            Assert.AreEqual(1, questions.Count);
            Assert.IsNotNull(questions.First().IncludedAnswerList);
            Assert.AreEqual(2, questions.First().IncludedAnswerList.Length);
        }

        [TestMethod]
        public void LoadsFromCache()
        {
            // Arrange
            _loader = new FilePersonaLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName, CacheName);
            _fakeCacheManger.Setup(x => x.Contains(CacheName)).Returns(true);

            // Act
            _loader.GetPersonas();

            //Assert
            _fakeCacheManger.Verify(x => x.Get(CacheName), Times.Once);
            _fakeCacheManger.Verify(x => x.Add(CacheName, It.IsAny<List<IRuleset>>()), Times.Never);
        }

        [TestMethod]
        public void AddsToCache()
        {
            // Arrange
            _loader = new FilePersonaLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName, CacheName);
            _fakeCacheManger.Setup(x => x.Contains(CacheName)).Returns(false);

            // Act
            _loader.GetPersonas();

            //Assert
            _fakeCacheManger.Verify(x => x.Get(CacheName), Times.Once);
            _fakeCacheManger.Verify(x => x.Add(CacheName, It.IsAny<List<IPersona>>()), Times.Once);
        }
    }
}
