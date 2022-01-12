using System.Net;

namespace TestNinja.Mocking
{
    public interface IWebClientDownloader
    {
        void DownloadFile(string url, string path);
    }

    public class WebClientDownloader : IWebClientDownloader
    {
        public void DownloadFile(string url, string path)
        {
            var client = new WebClient();
            
            client.DownloadFile(url, path);
        }
    }
}