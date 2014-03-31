using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping
{
    public interface IArticleScraper
    {
        Article Scrape(ArticleInfo articleInfo);
        Portal Portal { get; }
    }
}
