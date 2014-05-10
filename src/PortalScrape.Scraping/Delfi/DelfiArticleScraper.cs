using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiArticleScraper : IArticleScraper
    {
        public Portal Portal { get { return Portal.Delfi; } }

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
                Tags = GetTags(docNode),
                Keywords = GetKeywords(docNode),
                RelatedArticles = String.Join(", ", GetRelatedArticlesIds(docNode)),
            };
        }

        private static List<int> GetRelatedArticlesIds(HtmlNode docNode)
        {
            var relatedArticlesNodes =
                docNode.SelectNodes("//div[@id='artres-related-wrapper']/div[contains(@class, 'artres-related-pitem')]");

            if (relatedArticlesNodes == null)
            {
                return new List<int>();
            }

            var urlAttributes = relatedArticlesNodes.Select(e => e.Attributes["data-url"]).Where(u => u != null);
            var urls = urlAttributes.Select(e => e.Value);
            var ids = urls.Select(u => Convert.ToInt32(u.GetQueryParameterValueFromUrl("id"))).ToList();

            return ids;
        }

        private static string GetAuthorName(HtmlNode docNode)
        {
            var node = docNode.SelectSingleNode("//div[@class='delfi-author-name']") ??
                       docNode.SelectSingleNode("//div[@class='delfi-source-name']");

            if (node == null)
            {
                var t = 5;
            }

            return node == null ? null : node.InnerText;
        }

        private static string GetBody(HtmlNode docNode)
        {
            var paragraphs = docNode.SelectNodes("//div[@class='delfi-article-body']//p");

            var text = String.Join(" ", paragraphs.Elements().Select(e => e.InnerText));
            return text;
        }

        private static string GetKeywords(HtmlNode docNode)
        {
            var scripts = docNode.SelectNodes("//div[@id='header-ad-container']//script");
            var script = scripts.Elements().FirstOrDefault(e => e.InnerText != null && e.InnerText.Contains("__aokwd"));

            if (script == null) return null;

            var keywordString = script.InnerText.GetSubstringBetween("__aokwd=[", "]");

            return keywordString;
        }

        private string GetTags(HtmlNode docNode)
        {
            var nodeText = docNode.SelectSingleNode("//body/script[2]").InnerText;
            var startIndex = nodeText.IndexOf("tags=", StringComparison.InvariantCultureIgnoreCase) + 5;
            var endIndex = nodeText.IndexOf("'", startIndex, StringComparison.InvariantCultureIgnoreCase);
            var tagString = nodeText.Substring(startIndex, endIndex - startIndex);

            return tagString;
        }

        private DateTime GetDatePublished(HtmlNode docNode)
        {
            // meta itemprop="datePublished" content="
            //2014-03-19T19:11:35+0200
            var node = docNode.SelectSingleNode("//meta[@itemprop='datePublished']");
            return node.Attributes["content"].Value.ParseDateTime();
        }

        private DateTime GetDateModified(HtmlNode docNode)
        {
            // meta itemprop="dateModified" content="
            var node = docNode.SelectSingleNode("//meta[@itemprop='dateModified']");
            return node.Attributes["content"].Value.ParseDateTime();
        }
    }
}