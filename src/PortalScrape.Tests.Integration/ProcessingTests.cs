using NUnit.Framework;
using PortalScrape.Processing;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class ProcessingTests
    {
        [Test]
        public void OneFullCycle()
        {
            var cfg = new ProcessConfiguration(180, 20, 10);
            var process = new Process();
            process.Run(cfg);
        }
    }
}