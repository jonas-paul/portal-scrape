using NHibernate.Linq;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;
using LINQtoCSV;

namespace PortalScrape.Processing
{
    public class Export
    {
        public void Run()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var cc = new CsvContext();

                var articles = session.Query<Article>();
                cc.Write(articles, @"C:\articles.csv");

                var commments = session.Query<Comment>();
                cc.Write(commments, @"C:\comments.csv");
            }
        }
    }
}