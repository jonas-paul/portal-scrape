using System;
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
            var cfg = new ProcessConfiguration
            {
                Period = TimeSpan.FromHours(3),
                CommentsUpdateThreshold = 20,
                ArticleFetchThreshold = 10,
            };

            var process = new Process();
            process.Run(cfg);
        }
    }
}