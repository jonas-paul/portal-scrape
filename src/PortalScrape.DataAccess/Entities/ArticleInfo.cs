using System;

namespace PortalScrape.DataAccess.Entities
{
    [Serializable]
    public class ArticleInfo
    {
        public virtual Portal Portal { get; set; }
        public virtual string Id { get; set; }
        public virtual DateTime DateScraped { get; set; }

        public virtual string Url { get; set; }
        public virtual string Title { get; set; }
        public virtual int CommentCount { get; set; }
        public virtual DateTime? DatePublished { get; set; }

        public virtual bool HasArticleInDb { get; set; }
        public virtual int CommentCountInDb { get; set; }
        
        public override bool Equals(object obj)
        {
            var comment = obj as ArticleInfo;
            if (comment == null) return false;

            return comment.Portal == Portal && comment.Id == Id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode() * 23 + Portal.GetHashCode();
            }
        }
    }
}
