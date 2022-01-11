using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class ErrorLoggerTests
    {
        private ErrorLogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new ErrorLogger();
        }
        
        [Test]
        public void Log_WhenCalled_SetTheLastErrorProperty()
        {
            _logger.Log("a");
            
            Assert.That(_logger.LastError == "a");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Log_InvalidError_ThrowArgumentNullException(String error)
        {
            Assert.That(() => _logger.Log(error), Throws.ArgumentNullException);
        }

        [Test]
        public void Log_ValidError_RaiseErrorLoggedEvent()
        {
            var id = Guid.Empty;

            _logger.ErrorLogged += (sender, args) => id = args;
            _logger.Log("a");
            
            Assert.That(id != Guid.Empty);
        }
    }
}