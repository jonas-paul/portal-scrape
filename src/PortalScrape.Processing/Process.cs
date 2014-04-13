using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NHibernate.Linq;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping;
using PortalScrape.Scraping.Delfi;
using PortalScrape.Scraping.Lrytas;
using PortalScrape.Scraping.PenkMin;

namespace PortalScrape.Processing
{
    public class Process
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (Process));

        public void Run(ProcessConfiguration cfg)
        {
            _log.Info("Process started.");

            var articleOrders = new List<ArticleInfo>();
            var commentsOrders = new List<ArticleInfo>();

            var scrape = new CommonScraper();

            var sections = new List<Section>();
            sections.AddRange(Delfi.Sections.Take(2));
            sections.AddRange(Lrytas.Sections);
            sections.AddRange(PenkMin.Sections);

            using (var session = NHibernateHelper.OpenSession())
            {
                var currentInfos = session.Query<ArticleInfo>().ToList();

                foreach (var section in sections)
                {
                    _log.InfoFormat("Scraping section {0} in portal {1}...", section.Description, section.Portal);
                    var scrapedInfos = scrape.ArticleInfos(section, TimeSpan.FromMinutes(cfg.PeriodInMinutes)).Distinct().ToList();
                    _log.InfoFormat("{0} articles found.", scrapedInfos.Count);

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
                }

                session.Flush();

                _log.InfoFormat("{0} article orders issued.", articleOrders.Count);
                _log.InfoFormat("{0} comments orders issued.", commentsOrders.Count);

                foreach (var articleOrder in articleOrders)
                {
                    _log.InfoFormat("Scraping article '{0}' in portal {1}...", articleOrder.Title, articleOrder.Portal);
                    var article = scrape.Article(articleOrder);
                    if (article == null) continue;
                    session.SaveOrUpdate(article);
                }

                session.Flush();

                var commentsCounter = 0;

                foreach (var commentsOrder in commentsOrders)
                {
                    _log.InfoFormat("Scraping comments for article '{0}' in portal {1}...", commentsOrder.Title, commentsOrder.Portal);
                    var comments = scrape.Comments(commentsOrder, commentsOrder.CommentCountInDb, commentsOrder.CommentCount).Distinct().ToList();
                    comments.ForEach(session.SaveOrUpdate);

                    session.Flush();

                    commentsCounter += comments.Count;
                    _log.InfoFormat("Total comments scraped: {0}", commentsCounter);
                }
            }

            _log.Info("Process finished.");
        }
    }
}