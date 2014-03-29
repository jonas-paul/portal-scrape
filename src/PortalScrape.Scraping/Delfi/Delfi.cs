using System.Collections.Generic;

namespace PortalScrape.Scraping.Delfi
{
    public class Delfi
    {
        public const string MainHost = "http://www.delfi.lt";
        public const string RussianHost = "http://ru.delfi.lt";

        public static readonly List<Section> Sections = new List<Section>
            {
                new Section(MainHost, "news/daily/lithuania/", "LT::Delfi::Lietuvoje"),
                new Section(MainHost, "news/daily/world/", "LT::Delfi::Užsienyje"),
                new Section(MainHost, "news/daily/emigrants/", "LT::Delfi::Lietuviai Svetur"),
                new Section(MainHost, "news/daily/crime/", "LT::Delfi::Nusikaltimai ir nelaimės"),
                new Section(MainHost, "news/daily/education/", "LT::Delfi::Jaunimo Sodas"),
                new Section(MainHost, "news/daily/law/", "LT::Delfi::Teisė"),
                new Section(MainHost, "news/ringas/lit/", "LT::Nuomonių ringas::Lietuvos pjūvis"),
                new Section(MainHost, "news/ringas/politics/", "LT::Nuomonių ringas::Politiko akimis"),
                new Section(MainHost, "news/ringas/abroad/", "LT::Nuomonių ringas::Be sienų"),
                new Section(MainHost, "verslas/energetika/", "LT::Verslas::Energetika"),
                new Section(MainHost, "verslas/verslas/", "LT::Verslas::Verslo naujienos"),
                new Section(MainHost, "verslas/manolitai/", "LT::Verslas::Mano litai"),
                new Section(MainHost, "verslas/nekilnojamas-turtas/",
                                 "LT::Verslas::Nekilnojamasis turtas"),
                new Section(MainHost, "verslas/transportas/", "LT::Verslas::Transportas"),
                new Section(MainHost, "verslas/media/", "LT::Verslas::Media"),
                new Section(MainHost, "verslas/rinka/", "LT::Verslas::Rinkos"),
                new Section(MainHost, "verslas/ESparama/", "LT::Verslas::ES parama"),
                new Section(MainHost, "verslas/kaimas/", "LT::Verslas::Kaimo naujienos"),
                new Section(MainHost, "sportas/krepsinis/", "LT::Sportas::Krepšinis"),
                new Section(MainHost, "pilietis/voxpopuli/", "LT::Pilietis::Vox Populi"),
                new Section(RussianHost, "news/politics/", "RU::RU DELFI::Политика"),
                new Section(RussianHost, "news/es/", "RU::RU DELFI::Европейский дневник"),
            };
    }
}