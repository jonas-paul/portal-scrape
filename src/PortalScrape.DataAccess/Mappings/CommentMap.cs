using FluentNHibernate.Mapping;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.DataAccess.Mappings
{
    internal class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("Comments");

            CompositeId()
                .KeyProperty(x => x.Portal)
                .KeyProperty(x => x.RefNo);
            Map(x => x.DateScraped).Not.Nullable();

            Map(x => x.ArticleRefNo).Not.Nullable();
            Map(x => x.IpAddress).Not.Nullable();
            Map(x => x.DateCreated).Not.Nullable();

            Map(x => x.UserName).Not.Nullable();
            Map(x => x.CommentText).Not.Nullable();
            Map(x => x.InResponseToCommentId);
            Map(x => x.Upvotes);
            Map(x => x.DownVotes);
        }
    }
}