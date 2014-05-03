using FluentNHibernate.Mapping;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.DataAccess.Mappings
{
    internal class ArticleInfoMap : ClassMap<ArticleInfo>
    {
        public ArticleInfoMap()
        {
            Table("ArticleInfos");

            CompositeId(x => x.Id)
                .KeyProperty(x => x.Portal)
                .KeyProperty(x => x.ExternalId);
            Map(x => x.DateScraped).Not.Nullable();

            Map(x => x.Url).Not.Nullable();
            Map(x => x.CommentCount).Not.Nullable();
            Map(x => x.DatePublished);
            Map(x => x.Title).Not.Nullable();

            Map(x => x.CommentCountInDb).Formula("(SELECT COUNT(*) FROM Comments WHERE Comments.Portal = Portal AND Comments.ExternalId = ExternalId)");
            Map(x => x.HasArticleInDb).Formula("(SELECT 1 FROM Articles WHERE Articles.Portal = Portal AND Articles.ExternalId = ExternalId)");
        }
    }
}