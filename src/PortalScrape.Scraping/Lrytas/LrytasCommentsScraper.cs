using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Lrytas
{
    public class LrytasCommentsScraper : ICommentsScraper
    {
        public Portal Portal { get { return Portal.Lrytas; } }

        public List<Comment> ScrapeRange(ArticleInfo articleInfo, int from, int to)
        {
            var comments = new List<Comment>();

            var page = 1;
            var pagesToScrape = 100;

            while(page <= pagesToScrape)
            {
                var url = Lrytas.MainHost;
                url = url.AddQueryParameterToUrl("id", articleInfo.Id.ExternalId);
                url = url.AddQueryParameterToUrl("view", 6);
                url = url.AddQueryParameterToUrl("p", page);

                var docNode = Utilities.DownloadPage(url);

                if (page == 1)
                {
                    try
                    {
                        var pageNumberNodes = docNode.SelectNodes(".//div[@class='str-pages-div']/a");
                        var pageCount = Convert.ToInt32(pageNumberNodes[pageNumberNodes.Count - 2].InnerText);
                        pagesToScrape = pageCount - (@from / 25);
                    }
                    catch (Exception)
                    {
                        pagesToScrape = 1;
                    }
                }

                var commentNodes = docNode.SelectNodes(".//div[@class='comment']");
                var scrapedComments = commentNodes.Select(cn => ParseComment(cn, articleInfo.Id.ExternalId)).ToList();

                comments.AddRange(scrapedComments);
                page++;
            }

            return comments.Take(to - from + 1).ToList();
        }

        private Comment ParseComment(HtmlNode commentNode, string id)
        {
            var comment = new Comment();

            comment.ArticleId = id;
            comment.CommentText = HttpUtility.HtmlDecode(commentNode.SelectSingleNode("p").InnerText.Trim());
            comment.DateCreated =
                DateTime.ParseExact(commentNode.SelectSingleNode(".//div[@class='comment-time']").InnerText,
                    "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            comment.DateScraped = DateTime.UtcNow.AddHours(2);
            comment.Id.ExternalId = commentNode.SelectSingleNode(".//div[@class='comment-vert1']/a").Attributes["href"].Value
                .GetSubstringBetween("comr(", ",'");
            comment.IpAddress = commentNode.SelectSingleNode(".//div[@class='comment-ip']").InnerText
                .Replace("IP:", "").Trim();
            comment.Id.Portal = Portal.Lrytas;
            var commentTitle = commentNode.SelectSingleNode(".//div[@class='comment-nr']").InnerText;
            comment.UserName = HttpUtility.HtmlDecode(commentTitle.Substring(commentTitle.IndexOf('.') + 1).Trim());

            return comment;
        }
    }
}