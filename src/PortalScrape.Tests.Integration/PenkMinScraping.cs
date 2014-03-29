using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
    }
}
