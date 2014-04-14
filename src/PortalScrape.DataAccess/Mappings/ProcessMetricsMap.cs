using FluentNHibernate.Mapping;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.DataAccess.Mappings
{
    internal class ProcessMetricsMap : ClassMap<ProcessMetrics>
    {
        public ProcessMetricsMap()
        {
            Table("ProcessMetrics");

            Id(x => x.StartTime);
            Map(x => x.MinutesTaken).Not.Nullable();
            Map(x => x.ArticleOrders).Not.Nullable();
            Map(x => x.ArticlesScraped).Not.Nullable();
            Map(x => x.CommentsOrders).Not.Nullable();
            Map(x => x.CommentsScraped).Not.Nullable();
        }
    }
}