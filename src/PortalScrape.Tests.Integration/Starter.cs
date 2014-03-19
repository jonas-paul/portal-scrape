using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class Starter
    {
        [Test]
        public void First()
        {
            var delfi = new Delfi();

            var articles = new List<ArticleInfo>();

            var sw = new Stopwatch();
            sw.Start();

            foreach (var section in Delfi.Sections.Skip(1).Take(1))
            {
                for (var i = 1; i <= 1; i++)
                {
                    Console.WriteLine("Getting articles from section '{0}', page {1}..", section.Description, i);

                    try
                    {
                        articles.AddRange(new DelfiArticleInfoScraper().ScrapePage(section, i));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            Console.WriteLine("Finished getting {0} articles in {1}.", articles.Count, sw.Elapsed);

            File.WriteAllLines(@"D:\delfiarticleinfos.txt", articles.Select(a => a.ToString()));

            var aa = new List<DelfiArticle>();

            foreach (var delfiArticleInfo in articles)
            {
                try
                {
                    //aa.Add(delfiArticleInfo.GetArticle());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            File.WriteAllLines(@"D:\delfiarticles.txt", aa.Select(a => a.ToString()));
        }

        [Test]
        public void ScrapeArticle()
        {
            var articleInfo = new ArticleInfo
            {
                Url =
                    "http://www.delfi.lt/news/daily/lithuania/a-butkevicius-apie-nauja-lietuvos-kariuomenes-ginkluote-tai-yra-slapta.d?id=64317130"
            };

            var article = new DelfiArticleScraper().Scrape(articleInfo);
        }

        [Test]
        public void ScrapeArticleInfos()
        {
            var scraper = new DelfiArticleInfoScraper();
            var articleInfos = scraper.ScrapeForPeriod(Delfi.Sections.First(), TimeSpan.FromHours(8));
        }
    }
}