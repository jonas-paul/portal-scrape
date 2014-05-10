using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PortalScrape.Processing;

namespace PortalScrape.Tests.Integration.DataAccess
{
    [TestFixture]
    public class Exporter
    {
        [Test]
        public void Export()
        {
            var e = new Export();
            e.Run();
        }
    }
}
