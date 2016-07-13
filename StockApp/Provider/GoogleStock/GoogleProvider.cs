using StockApp.Models;
using StockApp.Provider.Helper;
using System.Collections.Generic;
using System.Linq;

namespace StockApp.Provider.GoogleStock
{
    public class GoogleProvider : IGoogleProvider
    {
        private const string QueryFormat = @"http://finance.google.com/finance/info?client=ig&q={0}";

        Quote IGoogleProvider.GetCurrentQuote(string symbol)
        {
            var rawData = QueryHelper.GetQuery<List<GoogleStock>>(string.Format(GoogleProvider.QueryFormat, symbol), 4);
            var stock = rawData.FirstOrDefault();
            Quote quote = new Quote();
            quote.Symbol = stock.Symbol;
            quote.LastTradePrice = decimal.Parse(stock.CurrentPrice);

            return quote;
        }
    }
}
