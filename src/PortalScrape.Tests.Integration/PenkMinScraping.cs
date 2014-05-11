using System;
using System.Linq;
using NUnit.Framework;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.PenkMin;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class PenkMinScraping
    {
        [Test]
        public void ArticleInfoPeriod()
        {
            var scr = new PenkMinArticleInfoScraper();
            var infos = scr.ScrapeForPeriod(PenkMin.Sections.Skip(1).First(), TimeSpan.FromDays(3));
        }

        [Test]
        public void ArticleInfo()
        {
            var scr = new PenkMinArticleInfoScraper();
            var infos = scr.ScrapePage(PenkMin.Sections.Skip(1).First(), 1);
        }

        [Test]
        public void Article()
        {
            var scr = new PenkMinArticleScraper();
            var article = scr.Scrape(new ArticleInfo { Url = "http://www.15min.lt/naujiena/aktualu/pasaulis/nato-palydovinese-nuotraukose-kaip-rusija-atitrauke-savo-pajegas-nuo-ukrainos-pasienio-57-425608?cf=df" });
        }

        [Test]
        public void Commments()
        {
            var scr = new PenkMinCommentsScraper();
            var comments = scr.ScrapeRange(new ArticleInfo { Url = "http://www.15min.lt/naujiena/aktualu/pasaulis/kabule-talibano-kovotojai-atakuoja-rinkimu-komisijos-bustine-57-415839?cf=vl" }, 2, 5);
        }
    }
}
