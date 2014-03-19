using FluentNHibernate.Mapping;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.DataAccess.Mappings
{
    internal class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Id(x => x.Id);
            Map(x => x.Portal);
            Map(x => x.RefNo);
            Map(x => x.DateScraped);

            Map(x => x.ArticleRefNo);
            Map(x => x.IpAddress);
            Map(x => x.DateCreated);

            Map(x => x.UserName);
            Map(x => x.CommentText);
            Map(x => x.InResponseToCommentId);
            Map(x => x.Upvotes);
            Map(x => x.DownVotes);
            
            Table("Comments");
        }
    }
}