using System.Collections.Generic;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Scraping.PenkMin
{
    public class PenkMin
    {
        public const string MainHost = "http://www.15min.lt";

        public static readonly List<Section> Sections = new List<Section>
            {
                new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/lietuva/", "lietuva"),
                new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/pasaulis/", "pasaulis"),
            };
    }
}
