using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NHibernate.Linq;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Processing
{
    public class Process
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (Process));
        private ProcessMetrics _metrics;

        public void Run(ProcessConfiguration cfg)
        {
            if (cfg == null)
            {
                cfg = ProcessConfiguration.FromAppConfig();
            }

            _metrics = new ProcessMetrics();
            _metrics.NotifyProcessStarted();

            _log.Info("Process started.");

            var articleOrders = new List<ArticleInfo>();
            var commentsOrders = new List<ArticleInfo>();

            var scrape = new CommonScraper();

            using (var session = NHibernateHelper.OpenSession())
            {
                var currentInfos = session.Query<ArticleInfo>().ToList();

                foreach (var section in cfg.Sections)
                {
                    _log.DebugFormat("Scraping section {0} in portal {1}...", section.Description, section.Portal);
                    var scrapedInfos = scrape.ArticleInfos(section, TimeSpan.FromHours(cfg.PeriodInHours)).Distinct().ToList();
                    _log.DebugFormat("{0} articles found.", scrapedInfos.Count);

                    foreach (var scrapedInfo in scrapedInfos)
                    {
                        var currentInfo = currentInfos.FirstOrDefault(ci => ci.Equals(scrapedInfo));

                        if (currentInfo != null)
                        {
                            if (!currentInfo.HasArticleInDb)
                            {
                                articleOrders.Add(scrapedInfo);
                            }

                            scrapedInfo.CommentCountInDb = currentInfo.CommentCountInDb;
                            if (scrapedInfo.CommentCount - currentInfo.CommentCountInDb >=
                                cfg.CommentsUpdateThreshold)
                            {
                                commentsOrders.Add(scrapedInfo);
                            }

                            session.Merge(scrapedInfo);
                        }
                        else
                        {
                            articleOrders.Add(scrapedInfo);
                            if (scrapedInfo.CommentCount >= cfg.ArticleFetchThreshold)
                            {
                                commentsOrders.Add(scrapedInfo);
                            }

                            session.Save(scrapedInfo);
                        }
                    }

                    session.Flush();
                }

                _metrics.ArticleOrders = articleOrders.Count;
                _metrics.CommentsOrders = commentsOrders.Count;

                _log.InfoFormat("{0} article orders issued.", articleOrders.Count);
                _log.InfoFormat("{0} comments orders issued.", commentsOrders.Count);

                foreach (var articleOrder in articleOrders)
                {
                    _log.DebugFormat("Scraping article '{0}' in portal {1}...", articleOrder.Title, articleOrder.Portal);
                    var article = scrape.Article(articleOrder);
                    if (article == null) continue;
                    session.SaveOrUpdate(article);
                    _metrics.ArticlesScraped++;
                }

                session.Flush();

                var commentsCounter = 0;

                foreach (var commentsOrder in commentsOrders)
                {
                    _log.DebugFormat("Scraping comments for article '{0}' in portal {1}...", commentsOrder.Title, commentsOrder.Portal);
                    var comments = scrape.Comments(commentsOrder, commentsOrder.CommentCountInDb, commentsOrder.CommentCount).Distinct().ToList();
                    comments.ForEach(session.SaveOrUpdate);

                    session.Flush();

                    commentsCounter += comments.Count;
                    _log.DebugFormat("Total comments scraped: {0}", commentsCounter);
                    _metrics.CommentsScraped += comments.Count;
                }

                _log.Info("Process finished.");

                _metrics.NotifyProcessFinished();
                session.Save(_metrics);
                session.Flush();
            }
        }
    }
}