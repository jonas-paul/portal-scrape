using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiSection
    {
        public DelfiSection(string host, string section, string description)
        {
            Host = host;
            Section = section;
            Description = description;
        }

        public string Host { get; private set; }
        public string Section { get; private set; }
        public string Description { get; private set; }

        public List<DelfiArticleInfo> GetArticles(int page)
        {
            var builder = new UriBuilder(Host);
            builder.Path += Section;
            var url = builder.ToString().AddQueryParameterToUrl("page", page);

            var docNode = Utilities.DownloadPage(url);
            var articleDivs = docNode.SelectNodes("//div[@class='category-headline-item']");

            return articleDivs.Select(ExtractBasicArticleInfo).ToList();
        }

        private static DelfiArticleInfo ExtractBasicArticleInfo(HtmlNode articleDiv)
        {
            var linkToArticle = articleDiv.ChildNodes.FindFirst("h3").ChildNodes.FindFirst("a");
            var dateDiv = articleDiv.SelectSingleNode("div[@class='headline-date']");

            var article = new DelfiArticleInfo();

            article.Url = linkToArticle.Attributes["href"].Value;
            article.Id = Convert.ToInt32(article.Url.GetQueryParameterValueFromUrl("id"));
            article.Title = linkToArticle.InnerText;
            article.DateString = dateDiv.InnerText;

            return article;
        }
    }
}