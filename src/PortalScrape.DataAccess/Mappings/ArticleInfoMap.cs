using FluentNHibernate.Mapping;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.DataAccess.Mappings
{
    internal class ArticleInfoMap : ClassMap<ArticleInfo>
    {
        public ArticleInfoMap()
        {
            Table("ArticleInfos");

            CompositeId()
                .KeyProperty(x => x.Portal)
                .KeyProperty(x => x.RefNo);
            Map(x => x.DateScraped).Not.Nullable();

            Map(x => x.Url).Not.Nullable();
            Map(x => x.CommentCount).Not.Nullable();
            Map(x => x.DatePublished).Not.Nullable();
            Map(x => x.Title).Not.Nullable();
        }
    }
}