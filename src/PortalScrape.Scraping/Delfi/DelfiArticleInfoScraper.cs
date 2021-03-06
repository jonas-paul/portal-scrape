﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using HtmlAgilityPack;
using log4net;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiArticleInfoScraper : IArticleInfoScraper
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (DelfiArticleInfoScraper));

        public Portal Portal { get { return Portal.Delfi; } }

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
                catch (CommonParsingException e)
                {
                }
                catch (Exception e)
                {
                    e.Data["articleDiv"] = articleDiv.OuterHtml;
                    _log.Error("An error occurred while parsing article info div.", e);
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
                throw new CommonParsingException("Delfi TV article");
            }

            articleInfo.Id.ExternalId = articleInfo.Url.GetQueryParameterValueFromUrl("id");
            articleInfo.Title = linkToArticle.InnerText;
            articleInfo.DatePublished = DelfiWordyDateParser.Parse(dateDiv.InnerText);
            articleInfo.DateScraped = DateTime.UtcNow.AddHours(2);
            articleInfo.Id.Portal = Portal.Delfi;
            articleInfo.CommentCount = commentCountNode == null ? 0 : Convert.ToInt32(commentCountNode.InnerText.TrimStart('(').TrimEnd(')'));

            var articleId = Convert.ToInt32(articleInfo.Url.GetQueryParameterValueFromUrl("id"));
            if (articleId == 0) throw new CommonParsingException("Article id not found");

            return articleInfo;
        }
    }
}