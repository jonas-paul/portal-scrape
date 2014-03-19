namespace PortalScrape.Scraping.Delfi
{
    public class DelfiSection
    {
        public DelfiSection(string host, string section, string description)
        {
            Host = host;
            Section = section;
            Description = description;
        }

        public string Host { get; private set; }
        public string Section { get; private set; }
        public string Description { get; private set; }
    }
}