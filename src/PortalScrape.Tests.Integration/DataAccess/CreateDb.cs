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
        public void First()
        {
            NHibernateHelper.ExportSchema();
        }

        [Test]
        public void SaveComment()
        {
            var fixture = new Fixture();
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var comment = fixture.Create<Comment>();
                    session.Save(comment);
                    transaction.Commit();
                }
            }
        }
    }
}