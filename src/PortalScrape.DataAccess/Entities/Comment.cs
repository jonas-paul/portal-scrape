﻿using System;

namespace PortalScrape.DataAccess.Entities
{
    public class Comment
    {
        public Comment()
        {
            Id = new EntityId();
        }

        public virtual EntityId Id { get; set; }
        public virtual DateTime DateScraped { get; set; }
        
        public virtual string ArticleExternalId { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string IpAddress { get; set; }

        public virtual string UserName { get; set; }
        public virtual string CommentText { get; set; }
        public virtual int InResponseToCommentId { get; set; }
        public virtual int Upvotes { get; set; }
        public virtual int DownVotes { get; set; }

        public override bool Equals(object obj)
        {
            var comment = obj as Comment;
            if (comment == null) return false;

            return comment.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}