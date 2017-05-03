// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = GoogleProvider.cs

namespace StockApp.Provider.GoogleStock
{
    using StockApp.Models;
    using StockApp.Provider.Helper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GoogleProvider : IGoogleProvider
    {
        private const string QueryFormat = @"http://finance.google.com/finance/info?client=ig&q={0}:{1}";

        async Task<StockInfo> IGoogleProvider.GetCurrentQuote(string exchange, string symbol)
        {
            var rawData = await QueryHelper.GetQuery<List<GoogleStock>>(string.Format(GoogleProvider.QueryFormat, exchange, symbol), 4);
            var stock = rawData.FirstOrDefault();
            StockInfo quote = new StockInfo();
            quote.Symbol = stock.Symbol;
            quote.LastTradePrice = decimal.Parse(stock.CurrentPrice);
            quote.ChangePercentage = double.Parse(stock.ChangePercentage);

            quote.LastUpdate = DateTime.Now;
            return quote;
        }
    }
}
