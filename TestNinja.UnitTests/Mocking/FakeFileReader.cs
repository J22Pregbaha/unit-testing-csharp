using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    public class FakeFileReader : IFileReader
    {
        public string Read(string path)
        {
            return ""; // It won't access the 'File' dependency cause it's just a fake
        }
    }
}