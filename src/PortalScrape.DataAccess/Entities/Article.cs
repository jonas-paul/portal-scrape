using System;

namespace PortalScrape.DataAccess.Entities
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual Portal Portal { get; set; }
        public virtual int RefNo { get; set; }
        public virtual DateTime DateScraped { get; set; }
        
        public virtual string Url { get; set; }
        public virtual int CommentCount { get; set; }
        public virtual DateTime DatePublished { get; set; }
        public virtual DateTime DateModified { get; set; }

        public virtual string Title { get; set; }
        public virtual string AuthorName { get; set; }
        public virtual string Body { get; set; }

        public virtual string Tags { get; set; }
        public virtual string Keywords { get; set; }
        public virtual string RelatedArticles { get; set; }

        public override bool Equals(object obj)
        {
            var comment = obj as Article;
            if (comment == null) return false;

            return comment.Portal == Portal && comment.RefNo == RefNo;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return RefNo.GetHashCode() * 23 + Portal.GetHashCode();
            }
        }
    }
}