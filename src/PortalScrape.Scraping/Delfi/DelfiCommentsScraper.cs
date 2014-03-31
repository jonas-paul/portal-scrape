using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiCommentsScraper : ICommentsScraper
    {
        public Portal Portal { get { return Portal.Delfi; } }

        public List<Comment> ScrapeRange(ArticleInfo articleInfo, int from, int to)
        {
            var comments = new List<Comment>();

            for (var i = from - 1; i <= to - 1; i += 20)
            {
                var url = articleInfo.Url;
                url = url.AddQueryParameterToUrl("com", 1);
                url = url.AddQueryParameterToUrl("s", 1);
                url = url.AddQueryParameterToUrl("no", i - 1);

                var docNode = Utilities.DownloadPage(url);
                var commentNodes = docNode.SelectNodes("//ul[@id='comments-list']/li");
                comments.AddRange(commentNodes.Select(cn => ParseComment(cn, articleInfo.Id)));
            }

            return comments.Take(to - from + 1).ToList();
        }

        private static Comment ParseComment(HtmlNode commentNode, string articleId)
        {
            var commentAnchor = commentNode.SelectSingleNode("a[@class='comment-list-comment-anchor']").Attributes["name"].Value;

            var authorNode = commentNode.SelectSingleNode("div[@class='comment-author']");
            var dateAndIp = authorNode.SelectSingleNode("div[contains(@class, 'comm-date')]").InnerText;
            var parts = dateAndIp.Split(new[] {"IP:"}, StringSplitOptions.None);
            var dateString = parts[0].Trim();
            var ipString = parts[1].Trim();

            var votesString = commentNode.SelectSingleNode("div[@class='comment-list-el-votes']/a").Attributes["rel"].Value;
            var votesParts = votesString.Split(new [] {":"}, StringSplitOptions.None);

            var comment = new Comment();
            comment.ArticleId = articleId;
            comment.CommentText = commentNode.SelectSingleNode("div[contains(@class, 'comment-body')]").InnerText.Trim();
            comment.DateCreated = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            comment.DateScraped = DateTime.UtcNow.AddHours(2);
            comment.DownVotes = Convert.ToInt32(votesParts[2]);
            comment.IpAddress = ipString;
            comment.Portal = Portal.Delfi;
            comment.Id = commentAnchor.Substring(1).Trim();
            comment.Upvotes = Convert.ToInt32(votesParts[1]);
            comment.UserName = authorNode.SelectSingleNode("h3").InnerText.Trim();

            var inReponseToNode = commentNode.SelectSingleNode("fieldset");
            if (inReponseToNode != null)
            {
                var responseUrl = inReponseToNode.Attributes["rel"].Value;
                var queryString = string.Join(string.Empty, responseUrl.Split('?').Skip(1));
                var qs = HttpUtility.ParseQueryString(queryString);
                comment.InResponseToCommentId = Convert.ToInt32(qs["q_id"]);
            }

            return comment;
        }
    }
}