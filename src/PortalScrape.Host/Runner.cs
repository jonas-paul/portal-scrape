﻿using System;
using System.Threading;
using PortalScrape.Processing;

namespace PortalScrape.Host
{
    public class Runner
    {
        private Timer _timer;

        public void Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            var cfg = new ProcessConfiguration
            {
                Period = TimeSpan.FromHours(3),
                CommentsUpdateThreshold = 20,
                ArticleFetchThreshold = 10,
            };

            _timer = new Timer(state => new Process().Run(cfg), null, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(30));
        }

        public void Stop()
        {
            _timer.Dispose();
        }
    }
}