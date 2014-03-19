using System;

namespace PortalScrape.DataAccess.Entities
{
    public class ArticleInfo
    {
        public virtual int Id { get; set; }
        public virtual Portal Portal { get; set; }
        public virtual int RefNo { get; set; }
        public virtual DateTime DateScraped { get; set; }

        public virtual string Url { get; set; }
        public virtual string Title { get; set; }
        public virtual int CommentCount { get; set; }
        public virtual DateTime DatePublished { get; set; }
    }
}
