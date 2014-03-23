using PortalScrape.Tests.Integration;
using PortalScrape.Tests.Integration.DataAccess;

namespace PortalScrape.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            //var starter = new Starter();
            //starter.ScrapeComments();

            var dbTests = new CreateDb();
            dbTests.ExportSchema();
        }
    }
}
