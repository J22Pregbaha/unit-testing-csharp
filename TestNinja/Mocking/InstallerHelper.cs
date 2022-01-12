using System.Net;

namespace TestNinja.Mocking
{
    public class InstallerHelper
    {
        private string _setupDestinationFile;
        private readonly IWebClientDownloader _webClientDownloader;

        public InstallerHelper(IWebClientDownloader webClientDownloader)
        {
            _webClientDownloader = webClientDownloader;
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            var url = $"http://example.com/{customerName}/{installerName}";
            try
            {
                _webClientDownloader.DownloadFile(url, _setupDestinationFile);

                return true;
            }
            catch (WebException)
            {
                return false; 
            }
        }
    }
}