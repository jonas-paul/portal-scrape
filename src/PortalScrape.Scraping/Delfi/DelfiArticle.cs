using System.Collections.Generic;
using Newtonsoft.Json;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiArticle
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

        public string DatePublished { get; set; }
        public string DateModified { get; set; }
        public string AuthorName { get; set; }
        public string Body { get; set; }

        public string Tags { get; set; }
        public string Keywords { get; set; }
        public List<int> RelatedArticles { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}