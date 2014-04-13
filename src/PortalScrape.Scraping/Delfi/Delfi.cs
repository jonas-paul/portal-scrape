using System.Collections.Generic;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Delfi
{
    public class Delfi
    {
        public const string MainHost = "http://www.delfi.lt";
        public const string RussianHost = "http://ru.delfi.lt";

        public static readonly List<Section> Sections = new List<Section>
            {
                new Section(Portal.Delfi, MainHost, "news/daily/lithuania/", "LT::Delfi::Lietuvoje"),
                new Section(Portal.Delfi, MainHost, "news/daily/world/", "LT::Delfi::Užsienyje"),
                new Section(Portal.Delfi, MainHost, "news/daily/emigrants/", "LT::Delfi::Lietuviai Svetur"),
                new Section(Portal.Delfi, MainHost, "news/daily/crime/", "LT::Delfi::Nusikaltimai ir nelaimės"),
                new Section(Portal.Delfi, MainHost, "news/daily/education/", "LT::Delfi::Jaunimo Sodas"),
                new Section(Portal.Delfi, MainHost, "news/daily/law/", "LT::Delfi::Teisė"),
                new Section(Portal.Delfi, MainHost, "news/ringas/lit/", "LT::Nuomonių ringas::Lietuvos pjūvis"),
                new Section(Portal.Delfi, MainHost, "news/ringas/politics/", "LT::Nuomonių ringas::Politiko akimis"),
                new Section(Portal.Delfi, MainHost, "news/ringas/abroad/", "LT::Nuomonių ringas::Be sienų"),
                new Section(Portal.Delfi, MainHost, "verslas/energetika/", "LT::Verslas::Energetika"),
                new Section(Portal.Delfi, MainHost, "verslas/verslas/", "LT::Verslas::Verslo naujienos"),
                new Section(Portal.Delfi, MainHost, "verslas/manolitai/", "LT::Verslas::Mano litai"),
                new Section(Portal.Delfi, MainHost, "verslas/nekilnojamas-turtas/", "LT::Verslas::Nekilnojamasis turtas"),
                new Section(Portal.Delfi, MainHost, "verslas/transportas/", "LT::Verslas::Transportas"),
                new Section(Portal.Delfi, MainHost, "verslas/media/", "LT::Verslas::Media"),
                new Section(Portal.Delfi, MainHost, "verslas/rinka/", "LT::Verslas::Rinkos"),
                new Section(Portal.Delfi, MainHost, "verslas/ESparama/", "LT::Verslas::ES parama"),
                new Section(Portal.Delfi, MainHost, "verslas/kaimas/", "LT::Verslas::Kaimo naujienos"),
                new Section(Portal.Delfi, MainHost, "sportas/krepsinis/", "LT::Sportas::Krepšinis"),
                new Section(Portal.Delfi, MainHost, "pilietis/voxpopuli/", "LT::Pilietis::Vox Populi"),
                //new Section(Portal.Delfi, RussianHost, "news/politics/", "RU::RU DELFI::Политика"),
                //new Section(Portal.Delfi, RussianHost, "news/es/", "RU::RU DELFI::Европейский дневник"),
            };
    }
}