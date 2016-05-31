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
    public class FileObligationLoaderTest
    {
        private Mock<IFilePathResolver> _fakeFilePathResolver;
        private Mock<ICacheManager> _fakeCacheManger;
        private ICacheManager _realCacheManger;
        private IObligationLoader _loader;

        private const string DirectoryPath = "..\\..\\_Data";
        private const string DirectoryName = "Obligations";
        private const string CacheName = "FileObligationLoaderTest";

        [TestInitialize]
        public void Setup()
        {
            _fakeFilePathResolver = new Mock<IFilePathResolver>();
            _fakeCacheManger = new Mock<ICacheManager>();
            _realCacheManger = new CacheManager();
            _fakeFilePathResolver.Setup(x => x.GetWorkingDirectory()).Returns(DirectoryPath);

            _loader = new FileObligationLoader(_realCacheManger, _fakeFilePathResolver.Object, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullCacheManager()
        {
            // Act
            _loader = new FileObligationLoader(null, _fakeFilePathResolver.Object, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullFilePathResolver()
        {
            // Act
            _loader = new FileObligationLoader(_fakeCacheManger.Object, null, DirectoryName, CacheName);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullCacheName()
        {
            // Act
            _loader = new FileObligationLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName,
                string.Empty);
        }

        [TestMethod]
        public void LoadObligations()
        {
            // Act
            var obligations = _loader.GetObligations();

            // Assert
            Assert.IsNotNull(obligations);
            Assert.AreEqual(3, obligations.Count);
        }

        [TestMethod]
        public void LoadsHtmlValuesFromCorrectly()
        {
            // Arrange
            _fakeFilePathResolver.Setup(x => x.GetWorkingDirectory()).Returns(DirectoryPath);

            // Act
            var obligations = _loader.GetObligations();

            // Assert
            Assert.IsNotNull(obligations);

            // property deserialised from json
            Assert.AreEqual("O1-DESCRIPTION", obligations[0].Description);

            // property loaded from file when not in json
            Assert.AreEqual("O2-DESCRIPTION", obligations[1].Description);

            // property deserialized from json when both json and file exist
            Assert.AreEqual("O3-DESCRIPTION", obligations[2].Description);
        }

        [TestMethod]
        public void LoadsFromCache()
        {
            // Arrange
            _loader = new FileObligationLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName, CacheName);
            _fakeCacheManger.Setup(x => x.Contains(CacheName)).Returns(true);

            // Act
            _loader.GetObligations();

            //Assert
            _fakeCacheManger.Verify(x => x.Get(CacheName), Times.Once);
            _fakeCacheManger.Verify(x => x.Add(CacheName, It.IsAny<List<IObligation>>()), Times.Never);
        }

        [TestMethod]
        public void AddsToCache()
        {
            // Arrange
            _loader = new FileObligationLoader(_fakeCacheManger.Object, _fakeFilePathResolver.Object, DirectoryName, CacheName);
            _fakeCacheManger.Setup(x => x.Contains(CacheName)).Returns(false);

            // Act
            _loader.GetObligations();

            //Assert
            _fakeCacheManger.Verify(x => x.Get(CacheName), Times.Once);
            _fakeCacheManger.Verify(x => x.Add(CacheName, It.IsAny<List<IObligation>>()), Times.Once);
        }
    }
}
