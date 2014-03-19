using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiArticleInfo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string DateString { get; set; }

        public DelfiArticle GetArticle()
        {
            var docNode = Utilities.DownloadPage(Url);

            var article = new DelfiArticle
                {
                    Id = Id,
                    Url = Url,
                    Title = Title,
                    DateModified = GetDateModified(docNode),
                    DatePublished = GetDatePublished(docNode),
                    Tags = GetTags(docNode),
                    Keywords = GetKeywords(docNode),
                    Body = GetBody(docNode),
                    AuthorName = GetAuthorName(docNode),
                    RelatedArticles = GetRelatedArticlesIds(docNode),
                };

            return article;
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

        private string GetDatePublished(HtmlNode docNode)
        {
            // meta itemprop="datePublished" content="
            var node = docNode.SelectSingleNode("//meta[@itemprop='datePublished']");
            return node.Attributes["content"].Value;
        }

        private string GetDateModified(HtmlNode docNode)
        {
            // meta itemprop="dateModified" content="
            var node = docNode.SelectSingleNode("//meta[@itemprop='dateModified']");
            return node.Attributes["content"].Value;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}