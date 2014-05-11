using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using log4net;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.PenkMin
{
    public class PenkMinArticleInfoScraper : IArticleInfoScraper
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(PenkMinArticleInfoScraper));

        public Portal Portal { get { return Portal.PenkMin; } }

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
            var url = builder.ToString();
            url = url.AddQueryParameterToUrl("psl", page);

            var docNode = Utilities.DownloadPage(url);
            var articleDivs = docNode.SelectNodes("//div[@class='article-row']");

            var result = new List<ArticleInfo>();

            foreach (var articleDiv in articleDivs)
            {
                try
                {
                    result.Add(ParseArticleInfoDiv(articleDiv));
                }
                catch (CommonParsingException e)
                {
                }
                catch (Exception e)
                {
                    e.Data["articleDiv"] = articleDiv.OuterHtml;
                    _log.Error("An error occurred while parsing article info div.", e);
                }
            }

            return result.Where(a => !a.Url.Contains("gallery") && !a.Url.Contains("/receptas/") && !a.Url.Contains("galerija")).ToList();
        }

        private static ArticleInfo ParseArticleInfoDiv(HtmlNode articleDiv)
        {
            var titleLink = articleDiv.SelectSingleNode(".//h4/a");
            var dateString = articleDiv.SelectSingleNode(".//em[@class='article-date']").InnerText.Trim();
            var commentLink = articleDiv.SelectSingleNode(".//p[@class='article-nfo']/a");
            var commentCount = 0;
            if (commentLink != null && !String.IsNullOrEmpty(commentLink.InnerText))
            {
                commentCount = Convert.ToInt32(commentLink.InnerText.Replace("&nbsp;", "").Trim());
            }
            
            var articleInfo = new ArticleInfo();
            articleInfo.Title = titleLink.InnerText;
            articleInfo.Url = titleLink.Attributes["href"].Value;
            articleInfo.DatePublished = DateTime.ParseExact(dateString, "yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture);
            articleInfo.DateScraped = DateTime.UtcNow.AddHours(2);
            articleInfo.CommentCount = commentCount;
            articleInfo.Id.Portal = Portal.PenkMin;
            articleInfo.Id.ExternalId = articleInfo.Url.Split(new[] {'?'}, StringSplitOptions.None)[0].Split(new[] {"-"},
                        StringSplitOptions.None).Last().Trim();

            return articleInfo;
        }
    }
}