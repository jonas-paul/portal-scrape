using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Processing
{
    public class Process
    {
        public void Run()
        {
            var articlePeriod = TimeSpan.FromHours(30);
            const int commentDifferenceToTriggerUpdate = 20;
            const int commentDifferenceToTriggerFetch = 20;

            var articleOrders = new List<ArticleInfo>();
            var commentsOrders = new List<ArticleInfo>();

            var cutOffTime = DateTime.UtcNow.AddHours(2).Add(-articlePeriod);
            var articleInfoScraper = new DelfiArticleInfoScraper();
            var articleScraper = new DelfiArticleScraper();
            var commentScraper = new DelfiCommentsScraper();

            using (var session = NHibernateHelper.OpenSession())
            {
                var currentInfos = session.Query<ArticleInfo>().Where(ai => ai.DatePublished > cutOffTime).ToList();

                foreach (var delfiSection in Delfi.Sections.Take(10))
                {
                    Console.WriteLine("Scraping section {0}...", delfiSection.Description);
                    var scrapedInfos = articleInfoScraper.ScrapeForPeriod(delfiSection, articlePeriod).Distinct().ToList();
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

                Console.WriteLine("{0} article orders issued.", articleOrders.Count);
                Console.WriteLine("{0} comments orders issued.", commentsOrders.Count);

                foreach (var articleOrder in articleOrders)
                {
                    Console.WriteLine("Scraping article '{0}'", articleOrder.Title);
                    var article = articleScraper.Scrape(articleOrder);
                    session.SaveOrUpdate(article);
                }

                foreach (var commentsOrder in commentsOrders)
                {
                    Console.WriteLine("Scraping comments for article '{0}'", commentsOrder.Title);
                    var comments = commentScraper.ScrapeRange(commentsOrder, commentsOrder.CommentCountInDb, commentsOrder.CommentCount).Distinct().ToList();
                    comments.ForEach(session.SaveOrUpdate);
                }

                session.Flush();
            }

            Console.WriteLine("Finished for now.");
        }
    }
}