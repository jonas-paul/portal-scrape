using System;
using System.Collections.Generic;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping
{
    public interface IArticleInfoScraper
    {
        List<ArticleInfo> ScrapeForPeriod(Section section, TimeSpan period);
        Portal Portal { get; }
    }
}
