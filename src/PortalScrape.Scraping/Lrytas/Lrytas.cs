using System.Collections.Generic;

namespace PortalScrape.Scraping.Lrytas
{
    public class Lrytas
    {
        public const string MainHost = "http://www.lrytas.lt/";

        public static readonly List<Section> Sections = new List<Section>
            {
                new Section(MainHost, "lietuvos-diena/aktualijos", "LT::Lrytas::Aktualijos"),
                new Section(MainHost, "verslas/rinkos-pulsas", "LT::Lrytas::VersloPulsas"),
            };
    }
}