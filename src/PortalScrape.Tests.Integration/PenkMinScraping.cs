using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.PenkMin;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class PenkMinScraping
    {
        [Test]
        public void ArticleInfo()
        {
            var scr = new PenkMinArticleInfoScraper();
            var infos = scr.ScrapeSection(PenkMin.Sections.Skip(1).First());
        }

        [Test]
        public void Article()
        {
            var scr = new PenkMinArticleScraper();
            var article = scr.Scrape(new ArticleInfo { Url = "http://www.15min.lt/naujiena/aktualu/pasaulis/kabule-talibano-kovotojai-atakuoja-rinkimu-komisijos-bustine-57-415839?cf=vl" });
        }
    }
}
