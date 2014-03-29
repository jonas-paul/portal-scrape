using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.PenkMin
{
    public class PenkMinArticleScraper
    {
        public Article Scrape(ArticleInfo articleInfo)
        {
            var docNode = Utilities.DownloadPage(articleInfo.Url);

            return new Article
            {
                Portal = articleInfo.Portal,
                Id = articleInfo.Id,
                CommentCount = articleInfo.CommentCount,
                Title = articleInfo.Title,
                Url = articleInfo.Url,
                DateScraped = DateTime.UtcNow.AddHours(2),
                AuthorName = GetAuthorName(docNode),
                Body = GetBody(docNode),
                DateModified = GetDateModified(docNode),
                DatePublished = GetDatePublished(docNode),
            };
        }

        private string GetBody(HtmlNode docNode)
        {
            var intro = docNode.SelectSingleNode("//div[@class='intro']").InnerText.Trim();
            var paragraphs = docNode.SelectNodes("//div[@itemprop='articleBody']/p").Select(p => p.InnerText.Trim());
            var parts = new List<string>();
            parts.Add(intro);
            parts.AddRange(paragraphs);

            var s = String.Join(" ", parts);

            return HttpUtility.HtmlDecode(s);
        }

        private string GetAuthorName(HtmlNode docNode)
        {
            return docNode.SelectSingleNode("//div[@class='author']/strong").InnerText;
        }

        private DateTime GetDatePublished(HtmlNode docNode)
        {
            // meta itemprop="datePublished" content="
            //2014-03-19T19:11:35+0200
            var node = docNode.SelectSingleNode("//meta[@itemprop='datePublished']");
            return node.Attributes["content"].Value.ParseDateTime();
        }

        private DateTime GetDateModified(HtmlNode docNode)
        {
            // meta itemprop="dateModified" content="
            var node = docNode.SelectSingleNode("//meta[@itemprop='dateModified']");
            return node.Attributes["content"].Value.ParseDateTime();
        }
    }
}
