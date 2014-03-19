using FluentNHibernate.Mapping;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.DataAccess.Mappings
{
    internal class ArticleInfoMap : ClassMap<ArticleInfo>
    {
        public ArticleInfoMap()
        {
            Id(x => x.Id);
            Map(x => x.Portal);
            Map(x => x.RefNo);
            Map(x => x.DateScraped);

            Map(x => x.Url);
            Map(x => x.CommentCount);
            Map(x => x.DatePublished);
            Map(x => x.Title);
            
            Table("ArticleInfos");
        }
    }
}
