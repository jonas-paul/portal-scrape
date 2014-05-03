using System;

namespace PortalScrape.DataAccess.Entities
{
    [Serializable]
    public class ArticleInfo
    {
        public ArticleInfo()
        {
            Id = new EntityId();
        }

        public virtual EntityId Id { get; set; }
        public virtual DateTime DateScraped { get; set; }

        public virtual string Url { get; set; }
        public virtual string Title { get; set; }
        public virtual int CommentCount { get; set; }
        public virtual DateTime? DatePublished { get; set; }

        public virtual bool HasArticleInDb { get; set; }
        public virtual int CommentCountInDb { get; set; }
        
        public override bool Equals(object obj)
        {
            var articleInfo = obj as ArticleInfo;
            if (articleInfo == null) return false;

            return articleInfo.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}