using System.Collections.Generic;
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
        private Mock<IVideoRepository> _videoRepository;

        [SetUp]
        public void Setup()
        {
            _fileReader = new Mock<IFileReader>();
            _videoRepository = new Mock<IVideoRepository>();
            _service = new VideoService(_fileReader.Object, _videoRepository.Object);
        }

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            _fileReader.Setup(fR => fR.Read("video.txt")).Returns("");

            var result = _service.ReadVideoTitle();
            
            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_AllVideosAreProcessed_ReturnEmptyString()
        {
            var videoIds = new List<Video>();
            _videoRepository.Setup(vR => vR.GetUnprocessedVideos()).Returns(videoIds);

            var result = _service.GetUnprocessedVideosAsCsv();
            
            Assert.That(result == "");
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_SomeVideosAreUnProcessed_ReturnStringOfUnprocessedVideoIds()
        {
            var videoIds = new List<Video>
            {
                new Video {Id = 1},
                new Video {Id = 2},
                new Video {Id = 3}
            };
            _videoRepository.Setup(vR => vR.GetUnprocessedVideos()).Returns(videoIds);

            var result = _service.GetUnprocessedVideosAsCsv();
            
            Assert.That(result == "1,2,3");
        }
    }
}