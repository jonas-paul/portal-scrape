using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;
using PortalScrape.Scraping.Delfi;

namespace PortalScrape.Scraping.PenkMin
{
    public class PenkMinArticleInfoScraper
    {
        public List<ArticleInfo> ScrapeSection(Section section)
        {
            var builder = new UriBuilder(section.Host);
            builder.Path += section.RelativeUrl;
            var url = builder.ToString();

            var docNode = Utilities.DownloadPage(url);
            var articleDivs = docNode.SelectNodes("//div[@class='article-content']");

            var result = new List<ArticleInfo>();

            foreach (var articleDiv in articleDivs.Take(100))
            {
                try
                {
                    result.Add(ParseArticleInfoDiv(articleDiv));
                }
                catch (Exception e)
                {
                    // TODO: log exception
                }
            }

            return result;
        }

        private static ArticleInfo ParseArticleInfoDiv(HtmlNode articleDiv)
        {
            var titleDiv = articleDiv.SelectSingleNode("div[@class='vl-article-title']");
            var commentIcon = titleDiv.SelectSingleNode("p/a/span[@class='comment-icon']");
            var commentCount = 0;
            if (commentIcon != null)
            {
                commentCount = Convert.ToInt32(commentIcon.ParentNode.InnerText.Replace("&nbsp;", "").Trim());
            }

            var articleInfo = new ArticleInfo();
            articleInfo.Title = titleDiv.SelectSingleNode("h3/span/a").InnerText;
            articleInfo.Url = titleDiv.SelectSingleNode("h3/span/a").Attributes["href"].Value;
            articleInfo.DateScraped = DateTime.UtcNow.AddHours(2);
            articleInfo.CommentCount = commentCount;
            articleInfo.Portal = Portal.PenkMin;
            articleInfo.RefNo =
                Convert.ToInt32(
                    articleInfo.Url.Split(new[] {'?'}, StringSplitOptions.None)[0].Split(new[] {"-"},
                        StringSplitOptions.None).Last());

            return articleInfo;
        }
    }
}