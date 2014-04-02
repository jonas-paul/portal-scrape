﻿using System;
using System.Collections.Generic;
using System.Linq;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping;
using PortalScrape.Scraping.Delfi;
using PortalScrape.Scraping.Lrytas;
using PortalScrape.Scraping.PenkMin;

namespace PortalScrape.Processing
{
    public class CommonScraper
    {
        readonly List<IArticleInfoScraper> _articleInfoScrapers = new List<IArticleInfoScraper>
            {
                new DelfiArticleInfoScraper(),
                new LrytasArticleInfoScraper(),
                new PenkMinArticleInfoScraper()
            };

        readonly List<IArticleScraper> _articleScrapers = new List<IArticleScraper>
            {
                new DelfiArticleScraper(),
                new LrytasArticleScraper(),
                new PenkMinArticleScraper()
            };

        readonly List<ICommentsScraper> _commentScrapers = new List<ICommentsScraper>
            {
                new DelfiCommentsScraper(),
                new LrytasCommentsScraper(),
                new PenkMinCommentsScraper()
            };

        public List<ArticleInfo> ArticleInfos(Section section, TimeSpan period)
        {
            try
            {
                return _articleInfoScrapers
                    .First(s => s.Portal == section.Portal)
                    .ScrapeForPeriod(section, period);
            }
            catch (Exception e)
            {
                // TODO: log error
            }

            return new List<ArticleInfo>();
        }

        public Article Article(ArticleInfo articleInfo)
        {
            try
            {
                return _articleScrapers
                    .First(s => s.Portal == articleInfo.Portal)
                    .Scrape(articleInfo);
            }
            catch (Exception)
            {
                // TODO: log error
            }

            return null;
        }

        public List<Comment> Comments(ArticleInfo articleInfo, int from, int to)
        {
            try
            {
                return _commentScrapers
                    .First(s => s.Portal == articleInfo.Portal)
                    .ScrapeRange(articleInfo, from, to);
            }
            catch (Exception e)
            {
                // TODO: log error
            }

            return new List<Comment>();
        }
    }
}