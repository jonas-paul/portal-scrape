using FluentNHibernate.Mapping;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.DataAccess.Mappings
{
    internal class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("Articles");

            CompositeId()
                .KeyProperty(x => x.Portal)
                .KeyProperty(x => x.RefNo);
            Map(x => x.DateScraped).Not.Nullable();

            Map(x => x.Url).Not.Nullable();
            Map(x => x.CommentCount).Not.Nullable();
            Map(x => x.DatePublished).Not.Nullable();
            Map(x => x.DateModified);

            Map(x => x.Title).Not.Nullable();
            Map(x => x.AuthorName);
            Map(x => x.Body).Not.Nullable();

            Map(x => x.Tags);
            Map(x => x.Keywords);
            Map(x => x.RelatedArticles);
        }
    }
}