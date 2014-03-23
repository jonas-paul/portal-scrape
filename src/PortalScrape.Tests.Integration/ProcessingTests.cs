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

        [Test]
        public void OneFullCycle()
        {
            var articleOrdersQueue = new BlockingCollection<ArticleInfo>();
            var commentOrdersQueue = new BlockingCollection<ArticleInfo>();

            var articleInfoWorker = new ArticleInfoWorker(articleOrdersQueue, commentOrdersQueue);
            articleInfoWorker.Work();

            var workers = new List<IWorker>
            {
                new ArticleWorker(articleOrdersQueue),
                new CommentWorker(commentOrdersQueue)
            };

            var tasks = workers.Select(worker => new Task(worker.Work)).ToList();

            tasks.ForEach(t => t.Start());

            Task.WaitAll(tasks.ToArray());
        }
    }
}