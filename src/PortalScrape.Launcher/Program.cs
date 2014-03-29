using System;
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

            //var dbTests = new CreateDb();
            //dbTests.ExportSchema();

            //var processingTests = new ProcessingTests();
            //processingTests.OneFullCycle();

            var penkMin = new PenkMinScraping();
            penkMin.ArticleInfo();

            //Console.ReadKey();
        }
    }
}
