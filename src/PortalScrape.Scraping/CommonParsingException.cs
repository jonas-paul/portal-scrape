using System;

namespace PortalScrape.Scraping
{
    public class CommonParsingException : Exception
    {
        public CommonParsingException(string message) : base(message)
        {
        }
    }
}