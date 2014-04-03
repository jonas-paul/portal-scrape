using Topshelf;

namespace PortalScrape.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Runner>(s =>
                {
                    s.ConstructUsing(name => new Runner());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.UseLog4Net();

                x.EnableServiceRecovery(rc => rc.RestartService(0));

                x.SetDescription("Portal scraping background process");
                x.SetDisplayName("PortalScrape");
                x.SetServiceName("PortalScrape");
            });
        }
    }
}