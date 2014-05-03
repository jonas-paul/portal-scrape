using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class EntityIdTests
    {
        [Test]
        public void GetHashCodesAreEqual()
        {
            var id1 = new EntityId {Portal = Portal.Lrytas, ExternalId = "432"};
            var id2 = new EntityId {Portal = Portal.Lrytas, ExternalId = "432"};

            Assert.That(id1.GetHashCode(), Is.EqualTo(id2.GetHashCode()));
        }

        [Test]
        public void GetHashCodesAreNotEqual()
        {
            var id1 = new EntityId {Portal = Portal.Lrytas, ExternalId = "432"};
            var id2 = new EntityId {Portal = Portal.Lrytas, ExternalId = "4323"};

            Assert.That(id1.GetHashCode(), Is.Not.EqualTo(id2.GetHashCode()));
        }

        [Test]
        public void GetHashCodesAreNotEqual2()
        {
            var id1 = new EntityId {Portal = Portal.PenkMin, ExternalId = "432"};
            var id2 = new EntityId {Portal = Portal.Lrytas, ExternalId = "432"};

            Assert.That(id1.GetHashCode(), Is.Not.EqualTo(id2.GetHashCode()));
        }
    }
}
