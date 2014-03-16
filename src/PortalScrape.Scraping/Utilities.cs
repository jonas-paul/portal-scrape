using System.Net.Http;
using HtmlAgilityPack;

namespace PortalScrape.Scraping
{
    public static class Utilities
    {
        public static HtmlNode DownloadPage(string url)
        {
            var client = new HttpClient();
            var task = client.GetStringAsync(url);
            task.Wait();
            var result = task.Result;

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            return doc.DocumentNode;
        }
    }
}