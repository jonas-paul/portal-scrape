using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PortalScrape.DataAccess.Entities;

namespace PortalScrape.Scraping.PenkMin
{
    public class PenkMinCommentsScraper
    {
        public List<Comment> ScrapeRange(ArticleInfo articleInfo, int from, int to)
        {
            var comments = new List<Comment>();

            for (var page = (from - 1) / 50 + 1; page <= (to - 1) / 50 + 1; page++)
            {
                var url = articleInfo.Url;
                url = url.AddQueryParameterToUrl("comments", "");
                url = url.AddQueryParameterToUrl("page", page);
                url = url.AddQueryParameterToUrl("order", "ASC");

                var docNode = Utilities.DownloadPage(url);

                var scriptNode = docNode.SelectNodes(".//script").FirstOrDefault(s => s.InnerText.Contains("article_comments"));
                var json = HttpUtility.UrlDecode(scriptNode.InnerText.GetSubstringBetween("var article_comments = ", "];") + "]");

                var commentsFromJson = JsonConvert.DeserializeObject<List<CommentFromJson>>(json);

                comments.AddRange(commentsFromJson.Select(c =>
                    new Comment
                    {
                        ArticleId = articleInfo.Id,
                        CommentText = c.content,
                        DateCreated = ParseRelativeDate(c.date),
                        DateScraped = DateTime.UtcNow.AddHours(2),
                        IpAddress = c.ip,
                        Portal = Portal.PenkMin,
                        Id = c.id,
                        UserName = c.name,
                    }));
            }

            return comments.Take(to - from + 1).ToList();
        }

        private class CommentFromJson
        {
            public string id { get; set; }
            public string name { get; set; }
            public string date { get; set; }
            public string content { get; set; }
            public string ip { get; set; }
        }

        private static DateTime ParseRelativeDate(string dateString)
        {
            var dateNames = new [] {"min.", "val.", "d."};
            dateString = dateString.Replace("prieš", "").Trim();

            var number = 0;
            TimeSpan timeSpan = TimeSpan.FromHours(0);

            foreach (var dateName in dateNames)
            {
                if (dateString.Contains(dateName))
                {
                    number = Convert.ToInt32(dateString.Replace(dateName, "").Trim());

                    switch (dateName)
                    {
                        case "min.":
                            timeSpan = TimeSpan.FromMinutes(number);
                            break;
                        case "val.":
                            timeSpan = TimeSpan.FromHours(number);
                            break;
                        case "d.":
                            timeSpan = TimeSpan.FromDays(number);
                            break;
                    }

                    break;
                }
            }

            return DateTime.UtcNow.Add(-timeSpan);
        }
    }
}
