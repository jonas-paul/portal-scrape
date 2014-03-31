using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Run()
        {
            var articlePeriod = TimeSpan.FromHours(30);
            const int commentDifferenceToTriggerUpdate = 20;
            const int commentDifferenceToTriggerFetch = 10;

            var articleOrders = new List<ArticleInfo>();
            var commentsOrders = new List<ArticleInfo>();

            var cutOffTime = DateTime.UtcNow.AddHours(2).Add(-articlePeriod);
            var articleInfoScrapers = new List<IArticleInfoScraper>
            {
                new DelfiArticleInfoScraper(),
                new LrytasArticleInfoScraper(),
                new PenkMinArticleInfoScraper()
            };

            var articleScrapers = new List<IArticleScraper>
            {
                new DelfiArticleScraper(),
                new LrytasArticleScraper(),
                new PenkMinArticleScraper()
            };

            var commentScrapers = new List<ICommentsScraper>
            {
                new DelfiCommentsScraper(),
                new LrytasCommentsScraper(),
                new PenkMinCommentsScraper()
            };

            var sections = new List<Section>();
            sections.AddRange(Delfi.Sections.Take(2));
            sections.AddRange(Lrytas.Sections);
            sections.AddRange(PenkMin.Sections);

            using (var session = NHibernateHelper.OpenSession())
            {
                //var currentInfos = session.Query<ArticleInfo>().Where(ai => ai.DatePublished > cutOffTime).ToList();
                var currentInfos = session.Query<ArticleInfo>().ToList();

                foreach (var section in sections)
                {
                    Console.WriteLine("Scraping section {0} in portal {1}...", section.Description, section.Portal);
                    var scrapedInfos = articleInfoScrapers.First(s => s.Portal == section.Portal)
                        .ScrapeForPeriod(section, articlePeriod).Distinct().ToList();
                    Console.WriteLine("{0} articles found.", scrapedInfos.Count);

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
                                commentDifferenceToTriggerUpdate)
                            {
                                commentsOrders.Add(scrapedInfo);
                            }

                            session.Merge(scrapedInfo);
                        }
                        else
                        {
                            articleOrders.Add(scrapedInfo);
                            if (scrapedInfo.CommentCount >= commentDifferenceToTriggerFetch)
                            {
                                commentsOrders.Add(scrapedInfo);
                            }

                            session.Save(scrapedInfo);
                        }
                    }
                }

                session.Flush();

                Console.WriteLine("{0} article orders issued.", articleOrders.Count);
                Console.WriteLine("{0} comments orders issued.", commentsOrders.Count);

                foreach (var articleOrder in articleOrders)
                {
                    Console.WriteLine("Scraping article '{0}' in portal {1}...", articleOrder.Title, articleOrder.Portal);
                    var article = articleScrapers.First(s => s.Portal == articleOrder.Portal).Scrape(articleOrder);
                    session.SaveOrUpdate(article);
                }

                session.Flush();

                foreach (var commentsOrder in commentsOrders)
                {
                    Console.WriteLine("Scraping comments for article '{0}' in portal {1}...", commentsOrder.Title, commentsOrder.Portal);
                    var comments = commentScrapers.First(s => s.Portal == commentsOrder.Portal).ScrapeRange(commentsOrder, commentsOrder.CommentCountInDb, commentsOrder.CommentCount).Distinct().ToList();
                    comments.ForEach(session.SaveOrUpdate);

                    session.Flush();
                }
            }

            Console.WriteLine("Finished for now.");
        }
    }
}