using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PortalScrape.DataAccess.Entities;
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
                Period = TimeSpan.FromHours(30),
                CommentsUpdateThreshold = 20,
                ArticleFetchThreshold = 10,
            };

            var process = new Process();
            process.Run(cfg);
        }
    }
}