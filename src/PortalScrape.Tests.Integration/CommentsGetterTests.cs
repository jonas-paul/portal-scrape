using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Tests.Integration
{
    [TestFixture]
    public class CommentsGetterTests
    {
        [Test]
        public void First()
        {
            var page =
                Utilities.DownloadPage(
                    "http://www.delfi.lt/news/daily/world/kryme-balsuoti-referendume-del-prisijungimo-prie-rusijos-zmones-verciami-net-gatveje.d?id=64284742&com=1&no=0&s=2");

            var commentNodes = page.SelectNodes("//ul[@id='comments-list']/li");

            foreach (var commentNode in commentNodes)
            {
                var comment = new Comment();

                var anchorNode = commentNode.SelectSingleNode("a[@class='comment-list-comment-anchor']");

                var id = Convert.ToInt32(anchorNode.Attributes["name"].Value.Substring(1));
            }
        }
    }
}
