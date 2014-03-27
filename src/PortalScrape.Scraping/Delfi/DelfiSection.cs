namespace PortalScrape.Scraping.Delfi
{
    public class DelfiSection
    {
        public DelfiSection(string host, string relativeUrl, string description)
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