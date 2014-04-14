using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using PortalScrape.Scraping;
using PortalScrape.Scraping.Delfi;
using PortalScrape.Scraping.Lrytas;
using PortalScrape.Scraping.PenkMin;

namespace PortalScrape.Processing
{
    public class ProcessConfiguration
    {
        public ProcessConfiguration(int periodInHours, int commentsUpdateThreshold, int articleFetchThreshold, Scope scope)
        {
            PeriodInHours = periodInHours;
            CommentsUpdateThreshold = commentsUpdateThreshold;
            ArticleFetchThreshold = articleFetchThreshold;

            Sections = new List<Section>();
            if (scope == Scope.AllSections)
            {
                Sections.AddRange(Delfi.Sections);
                Sections.AddRange(Lrytas.Sections);
                Sections.AddRange(PenkMin.Sections);
            }
            else
            {
                Sections.AddRange(Delfi.Sections.Take(2));
                Sections.AddRange(Lrytas.Sections.Take(2));
                Sections.AddRange(PenkMin.Sections.Take(2));
            }
        }

        public int PeriodInHours { get; private set; }
        public int CommentsUpdateThreshold { get; private set; }
        public int ArticleFetchThreshold { get; private set; }
        public List<Section> Sections { get; private set; }

        public static ProcessConfiguration FromAppConfig()
        {
            return new ProcessConfiguration(
                Convert.ToInt32(ConfigurationManager.AppSettings["PeriodInHours"]),
                Convert.ToInt32(ConfigurationManager.AppSettings["CommentsUpdateThreshold"]),
                Convert.ToInt32(ConfigurationManager.AppSettings["ArticleFetchThreshold"]),
                Scope.AllSections);
        }
    }

    public enum Scope
    {
        AllSections = 1,
        Minimal = 2
    }
}