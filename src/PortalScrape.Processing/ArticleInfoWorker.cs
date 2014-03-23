using System;
using System.Collections.Concurrent;
using System.Linq;
using NHibernate.Linq;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Processing
{
    public class ArticleInfoWorker
    {
        private readonly BlockingCollection<ArticleInfo> _articlesToScrapeQueue;
        private readonly BlockingCollection<ArticleInfo> _commentsToScrapeQueue;
        private TimeSpan _articlePeriod;
        private int _commentDifferenceToTriggerUpdate;
        private int _commentDifferenceToTriggerFetch;

        public ArticleInfoWorker(BlockingCollection<ArticleInfo> articlesToScrapeQueue,
            BlockingCollection<ArticleInfo> commentsToScrapeQueue)
        {
            _articlesToScrapeQueue = articlesToScrapeQueue;
            _commentsToScrapeQueue = commentsToScrapeQueue;
        }

        public void Work()
        {
            _articlePeriod = TimeSpan.FromDays(3);
            _commentDifferenceToTriggerUpdate = 20;
            _commentDifferenceToTriggerFetch = 20;

            var cutOffTime = DateTime.UtcNow.AddHours(2).Add(-_articlePeriod);
            var scraper = new DelfiArticleInfoScraper();

            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var currentInfos = session.Query<ArticleInfo>().Where(ai => ai.DatePublished > cutOffTime).ToList();

                foreach (var delfiSection in Delfi.Sections.Take(2))
                {
                    var scrapedInfos = scraper.ScrapeForPeriod(delfiSection, _articlePeriod).Distinct().ToList();

                    foreach (var scrapedInfo in scrapedInfos)
                    {
                        var currentInfo = currentInfos.FirstOrDefault(ci => ci.Equals(scrapedInfo));

                        if (currentInfo != null)
                        {
                            if (!currentInfo.HasArticleInDb)
                            {
                                _articlesToScrapeQueue.Add(scrapedInfo);
                            }
                            if (scrapedInfo.CommentCount - currentInfo.CommentCountInDb >= _commentDifferenceToTriggerUpdate)
                            {
                                _commentsToScrapeQueue.Add(scrapedInfo);
                            }
                            session.Merge(scrapedInfo);
                        }
                        else
                        {
                            _articlesToScrapeQueue.Add(scrapedInfo);
                            if (scrapedInfo.CommentCount >= _commentDifferenceToTriggerFetch)
                            {
                                _commentsToScrapeQueue.Add(scrapedInfo);
                            }
                        }
                    }
                }

                transaction.Commit();
            }
        }
    }
}