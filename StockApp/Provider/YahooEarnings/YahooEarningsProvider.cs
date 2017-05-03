// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = YahooEarningsProvider.cs

namespace StockApp.Provider.YahooEarnings
{
    using HtmlAgilityPack;
    using StockApp.Interfaces;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public class YahooEarningsProvider : IEarningsDateProvider
    {
        private const string QueryFormat = @"https://biz.yahoo.com/research/earncal/{0}/{1}.html";

        async Task<DateTime> IEarningsDateProvider.GetEarningsCallDate(string symbol)
        {
            var rawData = await Helper.QueryHelper.GetQuery<string>(string.Format(YahooEarningsProvider.QueryFormat, symbol.Substring(0, 1), symbol));

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(rawData);
            var nodes = document.DocumentNode.SelectNodes(string.Format("//a[@href=\"http://finance.yahoo.com/q?s={0}\"]", symbol.ToLower()));
            if (nodes.Count != 1)
            {
                throw new ApplicationException("There are not exactly 1 node");
            }

            var node = nodes.FirstOrDefault().ParentNode.NextSibling.NextSibling.NextSibling.FirstChild;
            Uri earningDateUri = new Uri(node.Attributes["href"].Value);
            var queryParameters = HttpUtility.ParseQueryString(earningDateUri.Query);
            string earningDate = queryParameters["ST"];
            DateTime dateToReturn;
            if (!DateTime.TryParseExact(earningDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dateToReturn))
            {
                throw new ApplicationException("Cannot parse date");
            }

            node = nodes.FirstOrDefault().ParentNode.NextSibling.NextSibling.FirstChild;
            string time = node.InnerHtml;

            if (time == "After Market Close")
            {
                dateToReturn = dateToReturn.AddHours(18);
            }
            else if (time == "Before Market Open")
            {
                dateToReturn = dateToReturn.AddHours(8);
            }

            return dateToReturn;
        }
    }
}
