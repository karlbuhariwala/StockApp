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
        private const string QueryFormat = @"https://finance.yahoo.com/calendar/earnings?symbol={0}";

        async Task<DateTime> IEarningsDateProvider.GetEarningsCallDate(string symbol)
        {
            var rawData = await Helper.QueryHelper.GetQuery<string>(string.Format(YahooEarningsProvider.QueryFormat, symbol));

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(rawData);
            var nodes = document.DocumentNode.SelectNodes("//div[@id=\"fin-cal-table\"]");
            if (nodes.Count != 1)
            {
                throw new ApplicationException("There are not exactly 1 node");
            }

            var tableNode = nodes.FirstOrDefault().SelectNodes("//tbody");
            var row = tableNode.FirstOrDefault().FirstChild;
            DateTime dateToReturn = DateTime.Now.AddYears(2);
            while (row != null)
            {
                var dateNode = row.FirstChild.NextSibling.NextSibling.NextSibling;
                var earningDate = dateNode.FirstChild.InnerHtml;
                DateTime tempDate;
                if (DateTime.TryParseExact(earningDate, "MMM dd, yyyy, h tt", null, System.Globalization.DateTimeStyles.None, out tempDate))
                {
                    if (tempDate < dateToReturn && tempDate > DateTime.Now)
                    {
                        dateToReturn = tempDate;
                    }
                }

                row = row.NextSibling;
            }

            return dateToReturn;
        }
    }
}
