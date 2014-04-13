using System.Collections.Generic;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.PenkMin
{
    public class PenkMin
    {
        public const string MainHost = "http://www.15min.lt";

        public static readonly List<Section> Sections = new List<Section>
        {
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/lietuva/", "lietuva"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/", "naujienos"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/pasaulis/", "pasaulis"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/uzsienietislt/", "uzsienietis"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/nusikaltimaiirnelaimes/", "nelaimes"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/politinis-poziuris/", "politika"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/karo-zona/", "karas"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/vilniaus-zinios/", "vilnius"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/aktualu/kauno-zinios/", "kaunas"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/verslas/", "verslas"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/verslas/finansai/", "finansai"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/verslas/bendroves/", "bendroves"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/verslas/energetika/", "energetika"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/verslas/transportas/", "transportas"),
            new Section(Portal.PenkMin, MainHost, "http://www.15min.lt/naujienos/verslas/zemes-ukis/", "zemes ukis"),
        };
    }
}