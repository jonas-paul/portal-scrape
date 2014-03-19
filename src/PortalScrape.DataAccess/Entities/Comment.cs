using System;

namespace PortalScrape.DataAccess.Entities
{
    public class Comment
    {
        public virtual int Id { get; set; }
        public virtual Portal Portal { get; set; }
        public virtual int RefNo { get; set; }
        public virtual DateTime DateScraped { get; set; }
        
        public virtual int ArticleRefNo { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string IpAddress { get; set; }

        public virtual string UserName { get; set; }
        public virtual string CommentText { get; set; }
        public virtual int InResponseToCommentId { get; set; }
        public virtual int Upvotes { get; set; }
        public virtual int DownVotes { get; set; }
    }
}