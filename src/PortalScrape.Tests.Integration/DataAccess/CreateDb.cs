using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Tests.Integration.DataAccess
{
    [TestFixture]
    public class CreateDb
    {
        [Test]
        [Explicit]
        public void ExportSchema()
        {
            NHibernateHelper.ExportSchema();
        }

        [Test]
        public void SaveComment()
        {
            var fixture = new Fixture();
            var comment = fixture.Create<Comment>();
            SaveEntity(comment);
        }

        [Test]
        public void SaveArticle()
        {
            var fixture = new Fixture();
            var comment = fixture.Create<Article>();
            SaveEntity(comment);
        }

        [Test]
        public void SaveArticleInfo()
        {
            var fixture = new Fixture();
            var comment = fixture.Create<ArticleInfo>();
            SaveEntity(comment);
        }

        [Test]
        public void LoadArticleInfo()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var articleInfo = session.Query<ArticleInfo>().Where(ai => ai.RefNo == 64345502).ToList();
            }
        }

        private void SaveEntity(object entity)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(entity);
                    transaction.Commit();
                }
            }
        }
    }
}