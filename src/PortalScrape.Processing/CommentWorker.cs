using System;
using System.Collections.Concurrent;
using System.Linq;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Processing
{
    public class CommentWorker : IWorker
    {
        private readonly BlockingCollection<ArticleInfo> _commentOrders;

        public CommentWorker(BlockingCollection<ArticleInfo> commentOrders)
        {
            _commentOrders = commentOrders;
        }

        public void Work()
        {
            var scraper = new DelfiCommentsScraper();

            while (true)
            {
                ArticleInfo commentsOrder;
                if (!_commentOrders.TryTake(out commentsOrder, 5000))
                {
                    break;
                }

                try
                {
                    var comments = scraper.ScrapeRange(commentsOrder, commentsOrder.CommentCountInDb, commentsOrder.CommentCount).Distinct().ToList();

                    // TODO: manage session and transaction
                    using (var session = NHibernateHelper.OpenSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        comments.ForEach(session.SaveOrUpdate);
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