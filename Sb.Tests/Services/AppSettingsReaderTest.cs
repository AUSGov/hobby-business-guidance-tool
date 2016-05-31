using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sb.Interfaces;
using Sb.Services;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AppSettingsReaderTest
    {
        private ISettingsReader _reader;

        [TestInitialize]
        public void Setup()
        {
            _reader = new AppSettingsReader();
        }

        [TestMethod]
        public void CanReadStringSetting_Exists()
        {
            // Act
            var result = _reader["TestStringSetting"];

            // Assert
            Assert.AreEqual("SomeString", result);
        }

        [TestMethod]
        public void CanReadStringSetting_NotExists()
        {
            // Act
            var result = _reader["Invalid"];

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CanReadIntSetting_Exists()
        {
            // Act
            var result = _reader.ReadInt("TestIntSetting", 123);

            // Assert
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void CanReadIntSetting_NotExists()
        {
            // Act
            var result = _reader.ReadInt("Invalid", 123);

            // Assert
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void CanReadBoolSetting_Exists()
        {
            // Act
            var result = _reader.ReadBool("TestBoolSetting", true);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CanReadBoolSetting_NotExists()
        {
            // Act
            var result = _reader.ReadBool("Invalid", true);

            // Assert
            Assert.AreEqual(true, result);
        }
    }
}

