using System;
using System.Diagnostics;

namespace PortalScrape.DataAccess.Entities
{
    public class ProcessMetrics
    {
        private Stopwatch _watch;

        public virtual DateTime StartTime { get; set; }
        public virtual int SecondsTaken { get; set; }
        public virtual int ArticleOrders { get; set; }
        public virtual int CommentsOrders { get; set; }
        public virtual int ArticlesScraped { get; set; }
        public virtual int CommentsScraped { get; set; }

        public virtual void NotifyProcessStarted()
        {
            _watch = new Stopwatch();
            _watch.Start();
            StartTime = DateTime.UtcNow.AddHours(2);
        }

        public virtual void NotifyProcessFinished()
        {
            _watch.Stop();
            SecondsTaken = (int)(_watch.ElapsedMilliseconds / 1000);
        }
    }
}