﻿using System;
using System.Collections.Generic;
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
            return ScrapeSection(section);
        }

        public List<ArticleInfo> ScrapeSection(Section section)
        {
            var builder = new UriBuilder(section.Host);
            builder.Path += section.RelativeUrl;
            var url = builder.ToString();

            var docNode = Utilities.DownloadPage(url);
            var articleDivs = docNode.SelectNodes("//div[@class='article-content']");

            var result = new List<ArticleInfo>();

            foreach (var articleDiv in articleDivs.Take(100))
            {
                try
                {
                    result.Add(ParseArticleInfoDiv(articleDiv));
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
            var titleDiv = articleDiv.SelectSingleNode("div[@class='vl-article-title']");
            var commentIcon = titleDiv.SelectSingleNode("p/a/span[@class='comment-icon']");
            var commentCount = 0;
            if (commentIcon != null)
            {
                commentCount = Convert.ToInt32(commentIcon.ParentNode.InnerText.Replace("&nbsp;", "").Trim());
            }

            var articleInfo = new ArticleInfo();
            articleInfo.Title = titleDiv.SelectSingleNode("h3/span/a").InnerText;
            articleInfo.Url = PenkMin.MainHost + titleDiv.SelectSingleNode("h3/span/a").Attributes["href"].Value;
            articleInfo.DateScraped = DateTime.UtcNow.AddHours(2);
            articleInfo.CommentCount = commentCount;
            articleInfo.Id.Portal = Portal.PenkMin;
            articleInfo.Id.ExternalId = articleInfo.Url.Split(new[] {'?'}, StringSplitOptions.None)[0].Split(new[] {"-"},
                        StringSplitOptions.None).Last().Trim();

            return articleInfo;
        }
    }
}