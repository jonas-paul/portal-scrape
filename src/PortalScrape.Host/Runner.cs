using System;
using System.Threading;
using log4net;
using PortalScrape.Processing;

namespace PortalScrape.Host
{
    public class Runner
    {
        private ILog _log = LogManager.GetLogger(typeof (Runner));
        private Timer _timer;

        public void Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            //var cfg = new ProcessConfiguration(2, 10, 20, Scope.Minimal);

            _timer = new Timer(state =>
            {
                try
                {
                    new Process().Run(null);
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(30));
        }

        public void Stop()
        {
            _timer.Dispose();
        }
    }
}