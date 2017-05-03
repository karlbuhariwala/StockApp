// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = GoogleFinancePageProvider.cs

namespace StockApp.Provider.GoogleFinancePage
{
    using HtmlAgilityPack;
    using StockApp.Interfaces;
    using StockApp.Provider.Helper;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class GoogleFinancePageProvider : ICurrentVolumeProvider
    {
        private const string QueryFormat = @"https://www.google.com/finance?q={0}%3A{1}";

        async Task<double> ICurrentVolumeProvider.GetCurrentVolume(string exchange, string symbol)
        {
            var rawData = await QueryHelper.GetQuery<string>(string.Format(GoogleFinancePageProvider.QueryFormat, exchange, symbol));
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(rawData);
            var nodes = document.DocumentNode.SelectNodes("//td[@data-snapfield=\"vol_and_avg\"]");
            if (nodes.Count != 1)
            {
                throw new ApplicationException("Nodes with price-panel is not 1");
            }

            var node = nodes.FirstOrDefault();
            string volume = node.NextSibling.NextSibling.InnerHtml;
            if (volume.Contains("/"))
            {
                volume = volume.Split(new char[] { '/' })[0];
            }

            volume = volume.Replace(",", string.Empty);

            double multiplier = 1;
            if (volume.EndsWith("M"))
            {
                volume = volume.Substring(0, volume.Length - 1);
                multiplier *= Math.Pow(10, 6);
            }

            double vol;
            if (!double.TryParse(volume, out vol))
            {
                throw new ApplicationException("Could not parse volume with M");
            }

            return vol * multiplier;
        }
    }
}
