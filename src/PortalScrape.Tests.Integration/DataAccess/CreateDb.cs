using NUnit.Framework;
using PortalScrape.DataAccess;

namespace PortalScrape.Tests.Integration.DataAccess
{
    [TestFixture]
    public class CreateDb
    {
        [Test]
        public void First()
        {
            NHibernateHelper.OpenSession();
        }
    }
}
