﻿using System;
using System.Linq;
using NUnit.Framework;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class DelfiScraping
    {
        [Test]
        public void ScrapeArticle()
        {
            var articleInfo = new ArticleInfo
            {
                Url = "http://www.delfi.lt/news/daily/crime/siuolaikines-vergoves-mastai-kelia-siauba.d?id=64756213"
            };

            var article = new DelfiArticleScraper().Scrape(articleInfo);
        }

        [Test]
        public void ScrapeArticleInfos()
        {
            var scraper = new DelfiArticleInfoScraper();
            var articleInfos = scraper.ScrapeForPeriod(Delfi.Sections.First(), TimeSpan.FromHours(8));
        }

        [Test]
        public void ScrapeComments()
        {
            var ai = new ArticleInfo
            {
                Url =
                    "http://www.delfi.lt/news/daily/education/dalis-sostines-priesmokyklinuku-isikurs-kitur.d?id=64297936",
                Id = { ExternalId = "64297936"}
            };
            var scraper = new DelfiCommentsScraper();
            var comments = scraper.ScrapeRange(ai, 1, 5);
        }
    }
}