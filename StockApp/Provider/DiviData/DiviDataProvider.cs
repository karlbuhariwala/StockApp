// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = DiviDataProvider.cs

namespace StockApp.Provider.DiviData
{
    using HtmlAgilityPack;
    using StockApp.Interfaces;
    using StockApp.Provider.Helper;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class DiviDataProvider : IExDividendDateProvider
    {
        private const string QueryFormat = @"https://dividata.com/stock/{0}";

        async Task<DateTime> IExDividendDateProvider.GetExDividendDate(string symbol)
        {
            string rawData = await QueryHelper.GetQuery<string>(string.Format(DiviDataProvider.QueryFormat, symbol));
            if (rawData == null)
            {
                return new DateTime();
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(rawData);
            var nodes = document.DocumentNode.SelectNodes("//abbr[@title=\"Stock must be purchased before this date in order to receive current dividend or stock split.\"]");
            if (nodes.Count != 1)
            {
                throw new ApplicationException("There are not exactly 1 node");
            }

            var node = nodes.FirstOrDefault().NextSibling;
            string exDividendDate = node.InnerHtml;
            if (exDividendDate == "N/A")
            {
                return new DateTime();
            }

            DateTime dateToReturn;
            if (!DateTime.TryParse(exDividendDate, out dateToReturn))
            {
                throw new ApplicationException("Cannot parse date");
            }

            return dateToReturn;
        }
    }
}
