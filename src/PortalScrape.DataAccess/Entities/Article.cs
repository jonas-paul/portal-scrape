using System;

namespace PortalScrape.DataAccess.Entities
{
    public class Article
    {
        public virtual EntityId Id { get; set; }
        public virtual DateTime DateScraped { get; set; }
        
        public virtual string Url { get; set; }
        public virtual int CommentCount { get; set; }
        public virtual DateTime? DatePublished { get; set; }
        public virtual DateTime? DateModified { get; set; }

        public virtual string Title { get; set; }
        public virtual string AuthorName { get; set; }
        public virtual string Body { get; set; }

        public virtual string Tags { get; set; }
        public virtual string Keywords { get; set; }
        public virtual string RelatedArticles { get; set; }

        public override bool Equals(object obj)
        {
            var article = obj as Article;
            if (article == null) return false;

            return article.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}