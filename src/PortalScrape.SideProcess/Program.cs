using PortalScrape.Tests.Integration;

namespace PortalScrape.SideProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var reprocess = new ReprocessArticles();
            reprocess.Reprocess();
        }
    }
}