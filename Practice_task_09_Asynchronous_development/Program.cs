using System.Net;

namespace Practice_task_09_Asynchronous_development
{
    internal class Program

    {
        static async Task Main(string[] args)
        {
            var downloader = new Downloader();
            var fastest = await downloader.RunFastestDownloadParallelAsync();

            downloader.DisplayWebsiteContent(fastest);
        }
        public class Downloader
        {

            private List<string> ListOfWebsites()
            {
                var output = new List<string>
            {
                "https://www.yahoo.com",
                "https://www.google.com",
                "https://www.microsoft.com"
            };

                return output;
            }

            public async Task<WebsiteDataModel> RunFastestDownloadParallelAsync()
            {
                List<string> websites = ListOfWebsites();
                var tasks = new List<Task<WebsiteDataModel>>();

                foreach (string site in websites)
                {
                    tasks.Add(DownloadWebsiteAsync(site));
                }

                var fastest = await Task.WhenAny(tasks);

                return fastest.Result;
            }

            private async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
            {
                var output = new WebsiteDataModel();
                var client = new WebClient();

                output.WebsiteUrl = websiteURL;
                output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

                return output;
            }

            public void DisplayWebsiteContent(WebsiteDataModel website)
            {
                Console.WriteLine($"Website \"{website.WebsiteUrl}\" content: \n" + website.WebsiteData);
            }
        }

        public class WebsiteDataModel
        {
            public string WebsiteUrl { get; set; } = "";
            public string WebsiteData { get; set; } = "";
        }
    }
}
