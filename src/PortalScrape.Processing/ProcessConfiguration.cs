using System;

namespace PortalScrape.Processing
{
    public class ProcessConfiguration
    {
        public TimeSpan Period { get; set; }
        public int CommentsUpdateThreshold { get; set; }
        public int ArticleFetchThreshold { get; set; }
    }
}