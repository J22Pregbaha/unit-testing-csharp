using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private Mock<IFileReader> _fileReader;
        private VideoService _service;

        [SetUp]
        public void Setup()
        {
            _fileReader = new Mock<IFileReader>();
            _service = new VideoService(_fileReader.Object);
        }

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            _fileReader.Setup(fR => fR.Read("video.txt")).Returns("");

            var result = _service.ReadVideoTitle();
            
            Assert.That(result, Does.Contain("error").IgnoreCase);
        }
    }
}