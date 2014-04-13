using System;
using System.Configuration;

namespace PortalScrape.Processing
{
    public class ProcessConfiguration
    {
        public ProcessConfiguration(int periodInMinutes, int commentsUpdateThreshold, int articleFetchThreshold)
        {
            PeriodInMinutes = periodInMinutes;
            CommentsUpdateThreshold = commentsUpdateThreshold;
            ArticleFetchThreshold = articleFetchThreshold;
        }

        public int PeriodInMinutes { get; private set; }
        public int CommentsUpdateThreshold { get; private set; }
        public int ArticleFetchThreshold { get; private set; }

        public static ProcessConfiguration FromAppConfig()
        {
            return new ProcessConfiguration(
                Convert.ToInt32(ConfigurationManager.AppSettings["PeriodInMinutes"]),
                Convert.ToInt32(ConfigurationManager.AppSettings["CommentsUpdateThreshold"]),
                Convert.ToInt32(ConfigurationManager.AppSettings["ArticleFetchThreshold"]));
        }
    }
}