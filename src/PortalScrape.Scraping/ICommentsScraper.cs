using System.Collections.Generic;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping
{
    public interface ICommentsScraper
    {
        List<Comment> ScrapeRange(ArticleInfo articleInfo, int from, int to);
        Portal Portal { get; }
    }
}
