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
        public void Run(ProcessConfiguration cfg)
        {
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
                    Console.WriteLine("Scraping section {0} in portal {1}...", section.Description, section.Portal);
                    var scrapedInfos = scrape.ArticleInfos(section, cfg.Period).Distinct().ToList();
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

                Console.WriteLine("{0} article orders issued.", articleOrders.Count);
                Console.WriteLine("{0} comments orders issued.", commentsOrders.Count);

                foreach (var articleOrder in articleOrders)
                {
                    Console.WriteLine("Scraping article '{0}' in portal {1}...", articleOrder.Title, articleOrder.Portal);
                    var article = scrape.Article(articleOrder);
                    session.SaveOrUpdate(article);
                }

                session.Flush();

                foreach (var commentsOrder in commentsOrders)
                {
                    Console.WriteLine("Scraping comments for article '{0}' in portal {1}...", commentsOrder.Title, commentsOrder.Portal);
                    var comments = scrape.Comments(commentsOrder, commentsOrder.CommentCountInDb, commentsOrder.CommentCount).Distinct().ToList();
                    comments.ForEach(session.SaveOrUpdate);

                    session.Flush();
                }
            }

            Console.WriteLine("Finished for now.");
        }
    }
}