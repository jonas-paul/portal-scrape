using System.Collections.Generic;

namespace PortalScrape.Scraping.Delfi
{
    public class Delfi
    {
        public const string MainHost = "http://www.delfi.lt";
        public const string RussianHost = "http://ru.delfi.lt";

        public readonly List<DelfiSection> Sections = new List<DelfiSection>
            {
                new DelfiSection(MainHost, "news/daily/lithuania/", "LT::Delfi::Lietuvoje"),
                new DelfiSection(MainHost, "news/daily/world/", "LT::Delfi::Užsienyje"),
                new DelfiSection(MainHost, "news/daily/emigrants/", "LT::Delfi::Lietuviai Svetur"),
                new DelfiSection(MainHost, "news/daily/crime/", "LT::Delfi::Nusikaltimai ir nelaimės"),
                new DelfiSection(MainHost, "news/daily/education/", "LT::Delfi::Jaunimo Sodas"),
                new DelfiSection(MainHost, "news/daily/law/", "LT::Delfi::Teisė"),
                new DelfiSection(MainHost, "news/ringas/lit/", "LT::Nuomonių ringas::Lietuvos pjūvis"),
                new DelfiSection(MainHost, "news/ringas/politics/", "LT::Nuomonių ringas::Politiko akimis"),
                new DelfiSection(MainHost, "news/ringas/abroad/", "LT::Nuomonių ringas::Be sienų"),
                new DelfiSection(MainHost, "verslas/energetika/", "LT::Verslas::Energetika"),
                new DelfiSection(MainHost, "verslas/verslas/", "LT::Verslas::Verslo naujienos"),
                new DelfiSection(MainHost, "verslas/manolitai/", "LT::Verslas::Mano litai"),
                new DelfiSection(MainHost, "verslas/nekilnojamas-turtas/",
                                 "LT::Verslas::Nekilnojamasis turtas"),
                new DelfiSection(MainHost, "verslas/transportas/", "LT::Verslas::Transportas"),
                new DelfiSection(MainHost, "verslas/media/", "LT::Verslas::Media"),
                new DelfiSection(MainHost, "verslas/rinka/", "LT::Verslas::Rinkos"),
                new DelfiSection(MainHost, "verslas/ESparama/", "LT::Verslas::ES parama"),
                new DelfiSection(MainHost, "verslas/kaimas/", "LT::Verslas::Kaimo naujienos"),
                new DelfiSection(MainHost, "sportas/krepsinis/", "LT::Sportas::Krepšinis"),
                new DelfiSection(MainHost, "pilietis/voxpopuli/", "LT::Pilietis::Vox Populi"),
                new DelfiSection(RussianHost, "news/politics/", "RU::RU DELFI::Политика"),
                new DelfiSection(RussianHost, "news/es/", "RU::RU DELFI::Европейский дневник"),
            };
    }
}