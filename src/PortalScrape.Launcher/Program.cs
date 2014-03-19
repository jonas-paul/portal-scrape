using PortalScrape.Tests.Integration;

namespace PortalScrape.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var starter = new Starter();
            starter.ScrapeArticle();
        }
    }
}
