using System;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Lrytas
{
    public class LrytasArticleScraper : IArticleScraper
    {
        public Portal Portal { get { return Portal.Lrytas; } }

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
            var authorNode = docNode.SelectSingleNode(".//strong[@itemprop='author']");
            if (authorNode == null)
            {
                authorNode = docNode.SelectSingleNode(".//div[@class='rtl-info']/strong");
            }
            return authorNode != null ? authorNode.InnerText.Trim() : null;
        }

        private DateTime? GetDatePublished(HtmlNode docNode)
        {
            var node = docNode.SelectSingleNode("//meta[@itemprop='datePublished']");
            if (node != null)
            {
                return node.Attributes["content"].Value.ParseDateTime();
            }

            node = docNode.SelectSingleNode(".//div[@class='rtl-info']/span");

            return node != null
                ? DateTime.ParseExact(node.InnerText.Split(new[] { ", atnaujinta " }, StringSplitOptions.None).First(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                : (DateTime?)null;
        }

        private DateTime? GetDateModified(HtmlNode docNode)
        {
            var node = docNode.SelectSingleNode("//meta[@itemprop='dateModified']");
            if (node != null)
            {
                return node.Attributes["content"].Value.ParseDateTime();
            }

            node = docNode.SelectSingleNode(".//div[@class='rtl-info']/span");

            if (node == null)
            {
                return null;
            }

            var parts = node.InnerText.Split(new[] { ", atnaujinta " }, StringSplitOptions.None);
            if (parts.Count() != 2)
            {
                return null;
            }

            return DateTime.ParseExact(parts[1], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
    }
}