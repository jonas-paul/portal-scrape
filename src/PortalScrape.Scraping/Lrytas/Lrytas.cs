using System.Collections.Generic;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Lrytas
{
    public class Lrytas
    {
        public const string MainHost = "http://www.lrytas.lt/";

        public static readonly List<Section> Sections = new List<Section>
            {
                new Section(Portal.Lrytas, MainHost, "lietuvos-diena/aktualijos", "LT::Lrytas::Aktualijos"),
                new Section(Portal.Lrytas, MainHost, "verslas/rinkos-pulsas", "LT::Lrytas::VersloPulsas"),
            };
    }
}