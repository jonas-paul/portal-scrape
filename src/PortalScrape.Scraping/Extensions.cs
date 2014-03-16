using System;
using System.Web;

namespace PortalScrape.Scraping
{
    public static class Extensions
    {
        public static string GetQueryParameterValueFromUrl(this string url, string paramName)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            return query[paramName];
        }

        public static string AddQueryParameterToUrl(this string url, string paramName, object paramValue)
        {
            var builder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query[paramName] = paramValue.ToString();
            builder.Query = query.ToString();
            return builder.ToString();
        }

        public static string GetSubstringBetween(this string s, string fromToken, string toToken)
        {
            var startIndex = s.IndexOf(fromToken, StringComparison.InvariantCultureIgnoreCase) + fromToken.Length;
            if (startIndex == -1) return null;
            var endIndex = s.IndexOf(toToken, startIndex, StringComparison.InvariantCultureIgnoreCase);
            if (endIndex == -1) return null;
            return s.Substring(startIndex, endIndex - startIndex);
        }
    }
}