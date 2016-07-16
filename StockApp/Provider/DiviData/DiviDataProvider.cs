using HtmlAgilityPack;
using StockApp.Provider.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider.DiviData
{
    public class DiviDataProvider : IDiviDataProvider
    {
        private const string QueryFormat = @"https://dividata.com/stock/{0}";

        async Task<DateTime> IDiviDataProvider.GetExDividendDate(string symbol)
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
