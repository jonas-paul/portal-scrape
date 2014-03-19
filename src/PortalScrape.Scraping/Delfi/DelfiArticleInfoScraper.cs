using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiArticleInfoScraper
    {
        public List<ArticleInfo> ScrapeForPeriod(DelfiSection section, TimeSpan period)
        {
            var page = 1;
            var articleInfos = new List<ArticleInfo>();
            var timeBottomLimit = DateTime.UtcNow.AddHours(2).Add(-period);

            while (true)
            {
                var articles = ScrapePage(section, page);
                var articlesInTimeRange = articles.Where(a => a.DatePublished > timeBottomLimit).ToList();
                articleInfos.AddRange(articlesInTimeRange);

                if (articles.Count() != articlesInTimeRange.Count() || page > 40) break;

                page++;
            }

            return articleInfos;
        }

        public List<ArticleInfo> ScrapePage(DelfiSection section, int page)
        {
            var builder = new UriBuilder(section.Host);
            builder.Path += section.Section;
            var url = builder.ToString().AddQueryParameterToUrl("page", page);

            var docNode = Utilities.DownloadPage(url);
            var articleDivs = docNode.SelectNodes("//div[@class='category-headline-item']");

            return articleDivs.Select(ParseArticleInfoDiv).ToList();
        }

        private static ArticleInfo ParseArticleInfoDiv(HtmlNode articleDiv)
        {
            var linkToArticle = articleDiv.SelectSingleNode("h3/a");
            var dateDiv = articleDiv.SelectSingleNode("div[@class='headline-date']");
            var commentCountNode = articleDiv.SelectSingleNode("h3/a[@class='commentCount']");

            var articleInfo = new ArticleInfo();

            articleInfo.Url = linkToArticle.Attributes["href"].Value;
            articleInfo.RefNo = Convert.ToInt32(articleInfo.Url.GetQueryParameterValueFromUrl("id"));
            articleInfo.Title = linkToArticle.InnerText;
            articleInfo.DatePublished = DelfiWordyDateParser.Parse(dateDiv.InnerText);
            articleInfo.DateScraped = DateTime.UtcNow.AddHours(2);
            articleInfo.Portal = Portal.Delfi;
            articleInfo.CommentCount = Convert.ToInt32(commentCountNode.InnerText.TrimStart('(').TrimEnd(')'));

            return articleInfo;
        }
    }
}