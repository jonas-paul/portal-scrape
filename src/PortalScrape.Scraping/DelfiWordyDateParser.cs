using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalScrape.Scraping
{
    public static class DelfiWordyDateParser
    {
        private static readonly Dictionary<string, int> LithuanianMonthNames = new Dictionary<string, int>
        {
            {"sausio", 1},
            {"vasario", 2},
            {"kovo", 3},
            {"balandžio", 4},
            {"gegužės", 5},
            {"birželio", 6},
            {"liepos", 7},
            {"rupjūčio", 8},
            {"rugsėjo", 9},
            {"spalio", 10},
            {"lapkričio", 11},
            {"gruodžio", 12},
        };

        private const string LithuanianMonthToken = "mėn.";
        private const string LithuanianDayToken= "d.";

        public static DateTime Parse(string dateString)
        {
            dateString = dateString.Trim();

            var year = Convert.ToInt32(dateString.Substring(0, 4));

            dateString = dateString.Substring(4);

            var parts = dateString.Split(new[] {LithuanianMonthToken, LithuanianDayToken}, StringSplitOptions.None);

            parts = parts.Select(p => p.Trim()).ToArray();

            var month = LithuanianMonthNames[parts[0]];

            var day = Convert.ToInt32(parts[1]);

            var timeParts = parts[2].Split(':');

            var hour = Convert.ToInt32(timeParts[0]);

            var minute = Convert.ToInt32(timeParts[1]);

            return new DateTime(year, month, day, hour, minute, 0);
        }
    }
}