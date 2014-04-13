using System.Collections.Generic;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Lrytas
{
    public class Lrytas
    {
        public const string MainHost = "http://www.lrytas.lt/";

        public static readonly List<Section> Sections = new List<Section>
        {
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/lietuvos-diena/aktualijos", "aktualijos"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/lietuvos-diena/nelaimes", "nelaimes"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/lietuvos-diena/kriminalai", "kriminalai"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/lietuvos-diena/studijos", "studijos"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/lietuvos-diena/politiko-zodis", "politika"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/verslas/rinkos-pulsas", "rinka"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/verslas/izvalgos-ir-nuomones", "nuomones"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/verslas/energetika", "energetika"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/verslas/mano-pinigai", "pinigai"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/pasaulis/ivykiai", "pasaulis"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/pasaulis/europa", "europa"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/pasaulis/konfliktai-ir-saugumas", "konfliktai"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/pasaulis/rytai-vakarai", "rytai-vakarai"),
            new Section(Portal.PenkMin, MainHost, "http://www.lrytas.lt/pasaulis/ivairenybes", "ivarus"),
        };
    }
}