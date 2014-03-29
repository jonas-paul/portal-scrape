namespace PortalScrape.Scraping
{
    public class Section
    {
        public Section(string host, string relativeUrl, string description)
        {
            Host = host;
            RelativeUrl = relativeUrl;
            Description = description;
        }

        public string Host { get; private set; }
        public string RelativeUrl { get; private set; }
        public string Description { get; private set; }
    }
}