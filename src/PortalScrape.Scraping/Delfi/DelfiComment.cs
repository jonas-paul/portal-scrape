using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalScrape.Scraping.Delfi
{
    public class DelfiComment
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string IpAddress { get; set; }
        public string UserName { get; set; }
        public string CommentText { get; set; }
        public string DateCreated { get; set; }
        public int InResponseToCommentId { get; set; }
        public int Upvotes { get; set; }
        public int DownVotes { get; set; }
    }
}
