using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiArticleInfoScraper
    {
        public List<ArticleInfo> ScrapeForPeriod(Section section, TimeSpan period)
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

        public List<ArticleInfo> ScrapePage(Section section, int page)
        {
            var builder = new UriBuilder(section.Host);
            builder.Path += section.RelativeUrl;
            var url = builder.ToString().AddQueryParameterToUrl("page", page);

            var docNode = Utilities.DownloadPage(url);
            var articleDivs = docNode.SelectNodes("//div[@class='category-headline-item']");

            var result = new List<ArticleInfo>();

            foreach (var articleDiv in articleDivs)
            {
                try
                {
                    result.Add(ParseArticleInfoDiv(articleDiv));
                }
                catch (Exception e)
                {
                    // TODO: log exception
                }
            }

            return result;
        }

        private static ArticleInfo ParseArticleInfoDiv(HtmlNode articleDiv)
        {
            var linkToArticle = articleDiv.SelectSingleNode("h3/a");
            var dateDiv = articleDiv.SelectSingleNode("div[@class='headline-date']");
            var commentCountNode = articleDiv.SelectSingleNode("h3/a[@class='commentCount']");

            var articleInfo = new ArticleInfo();

            articleInfo.Url = linkToArticle.Attributes["href"].Value;
            if (articleInfo.Url.Contains(@"/video/"))
            {
                throw new Exception("Delfi TV article");
            }

            articleInfo.RefNo = Convert.ToInt32(articleInfo.Url.GetQueryParameterValueFromUrl("id"));
            articleInfo.Title = linkToArticle.InnerText;
            articleInfo.DatePublished = DelfiWordyDateParser.Parse(dateDiv.InnerText);
            articleInfo.DateScraped = DateTime.UtcNow.AddHours(2);
            articleInfo.Portal = Portal.Delfi;
            articleInfo.CommentCount = commentCountNode == null ? 0 : Convert.ToInt32(commentCountNode.InnerText.TrimStart('(').TrimEnd(')'));

            var articleId = Convert.ToInt32(articleInfo.Url.GetQueryParameterValueFromUrl("id"));
            if (articleId == 0) throw new Exception("Article id not found");

            return articleInfo;
        }
    }
}