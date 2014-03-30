using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Lrytas
{
    public class LrytasArticleInfoScraper
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
            var url = builder.ToString().AddQueryParameterToUrl("p", page);

            var docNode = Utilities.DownloadPage(url);
            var articleDivs = docNode.SelectNodes("//div[@class='rubrika-new']");

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
            var linkToArticle = articleDiv.SelectSingleNode("a");
            var dateDiv = articleDiv.SelectSingleNode("div[@class='rubrika-posted']");
            var commentCountNode = articleDiv.SelectSingleNode(".//a[@class='k']");
            if (commentCountNode == null)
            {
                throw new Exception("Article id not found");
            }

            var articleInfo = new ArticleInfo();
            articleInfo.Url = new Uri (new Uri(Lrytas.MainHost), linkToArticle.Attributes["href"].Value).ToString();
            articleInfo.Id = commentCountNode.Attributes["href"].Value.GetSubstringBetween("=", "&");
            articleInfo.Title = articleDiv.SelectSingleNode("h2/a").InnerText;
            articleInfo.DatePublished = DateTime.ParseExact(dateDiv.InnerText, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            articleInfo.DateScraped = DateTime.UtcNow.AddHours(2);
            articleInfo.Portal = Portal.Lrytas;
            articleInfo.CommentCount = Convert.ToInt32(commentCountNode.InnerText);

            return articleInfo;
        }
    }
}