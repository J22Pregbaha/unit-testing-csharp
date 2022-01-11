using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            var service = new VideoService
            {
                FileReader = new FakeFileReader()
            };

            var result = service.ReadVideoTitle();
            
            Assert.That(result, Does.Contain("error").IgnoreCase);
        }
    }
}