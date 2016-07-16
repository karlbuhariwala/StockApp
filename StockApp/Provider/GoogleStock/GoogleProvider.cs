using StockApp.Models;
using StockApp.Provider.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockApp.Provider.GoogleStock
{
    public class GoogleProvider : IGoogleProvider
    {
        private const string QueryFormat = @"http://finance.google.com/finance/info?client=ig&q={0}:{1}";

        async Task<StockProfile> IGoogleProvider.GetCurrentQuote(string exchange, string symbol)
        {
            var rawData = await QueryHelper.GetQuery<List<GoogleStock>>(string.Format(GoogleProvider.QueryFormat, exchange, symbol), 4);
            var stock = rawData.FirstOrDefault();
            StockProfile quote = new StockProfile();
            quote.Symbol = stock.Symbol;
            quote.LastTradePrice = decimal.Parse(stock.CurrentPrice);

            return quote;
        }
    }
}
