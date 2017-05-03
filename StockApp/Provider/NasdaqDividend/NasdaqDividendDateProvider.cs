// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = NasdaqDividendDateProvider.cs

namespace StockApp.Provider.NasdaqDividend
{
    using HtmlAgilityPack;
    using StockApp.Interfaces;
    using StockApp.Provider.Helper;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class NasdaqDividendDateProvider : IExDividendDateProvider
    {
        private const string QueryFormat = "http://www.nasdaq.com/symbol/{0}/dividend-history";

        async Task<DateTime> IExDividendDateProvider.GetExDividendDate(string symbol)
        {
            string rawData = await QueryHelper.GetQuery<string>(string.Format(NasdaqDividendDateProvider.QueryFormat, symbol));
            if (rawData == null)
            {
                return new DateTime();
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(rawData);
            var nodes = document.DocumentNode.SelectNodes("//span[@id=\"quotes_content_left_dividendhistoryGrid_exdate_0\"]");
            if (nodes.Count != 1)
            {
                return new DateTime();
            }

            var node = nodes.FirstOrDefault();
            string exDividendDate = node.InnerHtml;

            DateTime dateToReturn;
            if (!DateTime.TryParse(exDividendDate, out dateToReturn))
            {
                throw new ApplicationException("Cannot parse date");
            }

            return dateToReturn;
        }
    }
}
