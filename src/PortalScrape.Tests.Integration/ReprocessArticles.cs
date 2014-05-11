using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using NUnit.Framework;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Processing;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class ReprocessArticles
    {
        [Test]
        public void Reprocess()
        {
            var scrape = new CommonScraper();

            using (var session = NHibernateHelper.OpenSession())
            {
                var articles = session.Query<Article>().Where(a => a.Id.Portal == Portal.Delfi && a.Tags == null).ToList();

                Console.WriteLine("articles to reprocess: {0}", articles.Count);
                var counter = 0;

                foreach (var article in articles)
                {
                    counter++;
                    Console.WriteLine("article #{0}", counter);

                    var sc =
                        scrape.Article(new ArticleInfo
                        {
                            Id = new EntityId {Portal = article.Id.Portal},
                            Url = article.Url
                        });

                    if (sc == null) continue;

                    article.Keywords = sc.Keywords;
                    article.Tags = sc.Tags;

                    session.Flush();
                }
            }
        }
    }
}