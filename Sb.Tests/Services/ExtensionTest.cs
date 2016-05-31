using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sb.Services;
using System.Collections.Specialized;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Extension
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void CanReadStringSetting_Exists()
        {
            // Arrange
            var nameValueCollection = new NameValueCollection {{"AA", "11"}, {"BB", "22"}, {"CC", "33"}};

            // Act
            var result = nameValueCollection.ToDictionary();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("11", result["AA"]);
            Assert.AreEqual("22", result["BB"]);
            Assert.AreEqual("33", result["CC"]);
        }

    }
}

