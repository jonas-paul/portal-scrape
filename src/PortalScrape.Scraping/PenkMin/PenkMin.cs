using System.Collections.Generic;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.PenkMin
{
    public class PenkMin
    {
        public const string MainHost = "http://www.15min.lt";

        public static readonly List<Section> Sections = new List<Section>
        {
            new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/lietuva/", "lietuva"),
            new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/pasaulis/", "pasaulis"),
            new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/uzsienietislt/", "uzsienietis"),
            new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/nusikaltimaiirnelaimes/", "nelaimes"),
            new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/politinis-poziuris/", "politika"),
            new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/karo-zona/", "karas"),
            new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/vilniaus-zinios/", "vilnius"),
            new Section(Portal.PenkMin, MainHost, "naujienos/aktualu/kauno-zinios/", "kaunas"),
            new Section(Portal.PenkMin, MainHost, "naujienos/verslas/finansai/", "finansai"),
            new Section(Portal.PenkMin, MainHost, "naujienos/verslas/bendroves/", "bendroves"),
            new Section(Portal.PenkMin, MainHost, "naujienos/verslas/energetika/", "energetika"),
            new Section(Portal.PenkMin, MainHost, "naujienos/verslas/transportas/", "transportas"),
            new Section(Portal.PenkMin, MainHost, "naujienos/verslas/zemes-ukis/", "zemes ukis"),
        };
    }
}