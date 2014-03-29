using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping
{
    public class Section
    {
        public Section(Portal portal, string host, string relativeUrl, string description)
        {
            Portal = portal;
            Host = host;
            RelativeUrl = relativeUrl;
            Description = description;
        }

        public Portal Portal { get; private set; }
        public string Host { get; private set; }
        public string RelativeUrl { get; private set; }
        public string Description { get; private set; }
    }
}