using System.Net;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private Mock<IWebClientDownloader> _webClientDownloader;
        private InstallerHelper _service;

        [SetUp]
        public void Setup()
        {
            _webClientDownloader = new Mock<IWebClientDownloader>();
            _service = new InstallerHelper(_webClientDownloader.Object);
        }

        [Test]
        public void DownloadInstaller_DownloadFails_ReturnsFalse()
        {
            _webClientDownloader.Setup(wCD => 
                wCD.DownloadFile(It.IsAny<string>(), It.IsAny<string>())) // I'm using any string here because Moq needs the inputs to be exact to work correctly
                .Throws<WebException>();
            var result = _service.DownloadInstaller("customer", "installer");
            
            Assert.That(result == false);
        }
        
        [Test]
        public void DownloadInstaller_DownloadPasses_ReturnTrue()
        {
            var result = _service.DownloadInstaller("customer", "installer");
            
            Assert.That(result, Is.True);
        }
    }
}