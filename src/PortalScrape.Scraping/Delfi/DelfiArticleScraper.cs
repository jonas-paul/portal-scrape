using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiArticleScraper
    {
        public Article Scrape(ArticleInfo articleInfo)
        {
            var docNode = Utilities.DownloadPage(articleInfo.Url);

            return new Article
            {
                Portal = articleInfo.Portal,
                RefNo = articleInfo.RefNo,
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

            var urlAttributes = relatedArticlesNodes.Select(e => e.Attributes["data-url"]).Where(u => u != null);
            var urls = urlAttributes.Select(e => e.Value);
            var ids = urls.Select(u => Convert.ToInt32(u.GetQueryParameterValueFromUrl("id"))).ToList();

            return ids;
        }

        private static string GetAuthorName(HtmlNode docNode)
        {
            var node = docNode.SelectSingleNode("//div[@class='delfi-author-name']");
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

            var keywordString = script.InnerText.GetSubstringBetween("__aokwd=[", "].");

            return keywordString;
        }

        private string GetTags(HtmlNode docNode)
        {
            var nodeText = docNode.SelectSingleNode("//body/script[2]").InnerText;
            var startIndex = nodeText.IndexOf("tags=", StringComparison.InvariantCultureIgnoreCase) + 5;
            var endIndex = nodeText.IndexOf("');", StringComparison.InvariantCultureIgnoreCase);
            var tagString = nodeText.Substring(startIndex, endIndex - startIndex);

            return tagString;
        }

        private DateTime GetDatePublished(HtmlNode docNode)
        {
            // meta itemprop="datePublished" content="
            //2014-03-19T19:11:35+0200
            var node = docNode.SelectSingleNode("//meta[@itemprop='datePublished']");
            return ParseDateTime(node.Attributes["content"].Value);
        }

        private DateTime GetDateModified(HtmlNode docNode)
        {
            // meta itemprop="dateModified" content="
            var node = docNode.SelectSingleNode("//meta[@itemprop='dateModified']");
            return ParseDateTime(node.Attributes["content"].Value);
        }

        private static DateTime ParseDateTime(string dateTimeString)
        {
            var s = dateTimeString.Substring(0, dateTimeString.Length - 5);
            return DateTime.ParseExact(s, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}