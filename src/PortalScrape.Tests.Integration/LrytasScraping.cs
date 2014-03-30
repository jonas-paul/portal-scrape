using System;
using System.Linq;
using NUnit.Framework;
using PortalScrape.Scraping.Lrytas;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class LrytasScraping
    {
        [Test]
        public void ArticleInfo()
        {
            var scraper = new LrytasArticleInfoScraper();
            scraper.ScrapeForPeriod(Lrytas.Sections.First(), TimeSpan.FromDays(3));
        }
    }
}
