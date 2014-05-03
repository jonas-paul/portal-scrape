using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.PenkMin
{
    public class PenkMinArticleScraper : IArticleScraper
    {
        public Portal Portal { get { return Portal.PenkMin; } }

        public Article Scrape(ArticleInfo articleInfo)
        {
            var docNode = Utilities.DownloadPage(articleInfo.Url);

            return new Article
            {
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
            var introNode = docNode.SelectSingleNode("//div[@class='intro']");
            var intro = introNode != null ? introNode.InnerText.Trim() : "";
            var paragraphNodes = docNode.SelectNodes("//div[@itemprop='articleBody']/p");
            var paragraphs = new List<string>();
            if (paragraphNodes != null)
            {
                paragraphs = paragraphNodes.Select(p => p.InnerText.Trim()).ToList();
            }
            var parts = new List<string>();
            parts.Add(intro);
            parts.AddRange(paragraphs);

            var s = String.Join(" ", parts);

            return HttpUtility.HtmlDecode(s);
        }

        private string GetAuthorName(HtmlNode docNode)
        {
            var node = docNode.SelectSingleNode("//div[@class='author']/strong");
            return node != null ? node.InnerText.Trim() : null;
        }

        private DateTime GetDatePublished(HtmlNode docNode)
        {
            // meta itemprop="datePublished" content="
            //2014-03-19T19:11:35+0200
            var node = docNode.SelectSingleNode("//meta[@itemprop='datePublished']");
            return node.Attributes["content"].Value.ParseDateTime();
        }

        private DateTime? GetDateModified(HtmlNode docNode)
        {
            // meta itemprop="dateModified" content="
            var node = docNode.SelectSingleNode("//meta[@itemprop='dateModified']");
            return node != null ? node.Attributes["content"].Value.ParseDateTime() : (DateTime?)null;
        }
    }
}
