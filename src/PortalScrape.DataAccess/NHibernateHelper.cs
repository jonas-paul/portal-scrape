using System.Data.SqlClient;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace PortalScrape.DataAccess
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void ExportSchema()
        {
            InitializeSessionFactory(true);
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)

                    InitializeSessionFactory(false);
                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory(bool exportSchema)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "PCJONPAUD1";
            builder.InitialCatalog = "PortalScrape";
            builder.IntegratedSecurity = true;

            var config = Fluently.Configure().Database(
                MsSqlConfiguration.MsSql2008.ConnectionString(builder.ToString()).ShowSql()
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>()
                    .Conventions.Add<StringColumnLengthConvention >());

            if (exportSchema)
            {
                config.ExposeConfiguration(c => new SchemaExport(c).Create(true, true));
            }
            
            _sessionFactory = config.BuildSessionFactory();
        }
    }
}