using System;
using System.Collections.Concurrent;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Processing
{
    public class ArticleWorker : IWorker
    {
        private readonly BlockingCollection<ArticleInfo> _articleOrders;

        public ArticleWorker(BlockingCollection<ArticleInfo> articleOrders)
        {
            _articleOrders = articleOrders;
        }

        public void Work()
        {
            var scraper = new DelfiArticleScraper();

            while (true)
            {
                ArticleInfo articleOrder;
                if (!_articleOrders.TryTake(out articleOrder, 5000))
                {
                    break;
                }

                try
                {
                    var article = scraper.Scrape(articleOrder);

                    // TODO: manage session and transaction
                    using (var session = NHibernateHelper.OpenSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(article);
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    // TODO: handle exception instead of rethrowing
                    throw;
                }
            }
        }
    }
}