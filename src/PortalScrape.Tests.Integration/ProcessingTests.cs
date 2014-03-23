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
        public void GetInfosAndPersist()
        {
            var articleOrdersQueue = new BlockingCollection<ArticleInfo>();
            var commentOrdersQueue = new BlockingCollection<ArticleInfo>();
            var worker = new ArticleInfoWorker(articleOrdersQueue, commentOrdersQueue);
            worker.Work();
        }
    }
}