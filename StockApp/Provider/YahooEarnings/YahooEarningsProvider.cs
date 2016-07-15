using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StockApp.Provider.YahooEarnings
{
    public class YahooEarningsProvider : IYahooEarningsProvider
    {
        private const string QueryFormat = @"https://biz.yahoo.com/research/earncal/{0}/{1}.html";

        DateTime IYahooEarningsProvider.GetEarningsCallDate(string symbol)
        {
            var rawData = Helper.QueryHelper.GetQuery<string>(string.Format(YahooEarningsProvider.QueryFormat, symbol.Substring(0, 1), symbol));

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
