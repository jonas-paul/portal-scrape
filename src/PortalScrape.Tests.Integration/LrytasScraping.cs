using System;
using System.Linq;
using NUnit.Framework;
using PortalScrape.DataAccess.Entities;
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

        [Test]
        public void Article()
        {
            var scr = new LrytasArticleScraper();
            var article = scr.Scrape(new ArticleInfo { Url = "http://www.lrytas.lt/lietuvos-diena/aktualijos/narystes-nato-desimtmetis-pazymetas-begimu-aviacijos-bazeje.htm#.Uzf50PmSyyk" });
        }

        [Test]
        public void Commments()
        {
            var scr = new LrytasCommentsScraper();
            var comments = scr.ScrapeRange(new ArticleInfo { Id = {ExternalId = "13960425731395759508" } }, 270, 500);
        }
    }
}