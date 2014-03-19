using FluentNHibernate.Mapping;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.DataAccess.Mappings
{
    internal class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Id(x => x.Id);
            Map(x => x.Portal);
            Map(x => x.RefNo);
            Map(x => x.DateScraped);

            Map(x => x.Url);
            Map(x => x.CommentCount);
            Map(x => x.DatePublished);
            Map(x => x.DateModified);

            Map(x => x.Title);
            Map(x => x.AuthorName);
            Map(x => x.Body);

            Map(x => x.Tags);
            Map(x => x.Keywords);
            Map(x => x.RelatedArticles);
            
            Table("Articles");
        }
    }
}
