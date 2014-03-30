using System;
using System.Linq;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Lrytas
{
    public class LrytasArticleScraper
    {
        public Article Scrape(ArticleInfo articleInfo)
        {
            var docNode = Utilities.DownloadPage(articleInfo.Url);

            return new Article
            {
                Portal = Portal.Lrytas,
                Id = articleInfo.Id,
                CommentCount = articleInfo.CommentCount,
                Title = articleInfo.Title,
                Url = articleInfo.Url,
                DateScraped = DateTime.UtcNow.AddHours(2),
                AuthorName = GetAuthorName(docNode),
                Body = GetBody(docNode),
                DateModified = GetDateModified(docNode),
                DatePublished = GetDatePublished(docNode),
            };
        }

        private string GetBody(HtmlNode docNode)
        {
            var paragraphs = docNode.SelectNodes(".//p[@class='text-15-str']").Select(e => e.InnerText.Trim());
            return String.Join(" ", paragraphs);
        }

        private string GetAuthorName(HtmlNode docNode)
        {
            return docNode.SelectSingleNode(".//strong[@itemprop='author']").InnerText;
        }

        private DateTime GetDatePublished(HtmlNode docNode)
        {
            var node = docNode.SelectSingleNode("//meta[@itemprop='datePublished']");
            return node.Attributes["content"].Value.ParseDateTime();
        }

        private DateTime GetDateModified(HtmlNode docNode)
        {
            var node = docNode.SelectSingleNode("//meta[@itemprop='dateModified']");
            return node != null ? node.Attributes["content"].Value.ParseDateTime() : new DateTime();
        }
    }
}
