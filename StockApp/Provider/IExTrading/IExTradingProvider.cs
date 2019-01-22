// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockProvider.cs

namespace StockApp.Provider.IExTradingProvider
{
    using Interfaces;
    using Newtonsoft.Json;
    using StockApp.Models;
    using StockApp.Provider.GoogleStock;
    using System.Threading.Tasks;

    public class IExTradingProvider : IStockProvider
    {
        private const string Endpoint = "https://api.iextrading.com/1.0/";

        private const string StatApi = "stock/{0}/quote";

        public async Task<StockInfo> GetStockInfo(string exchange, string symbol)
        {
            var stockInfo = new StockInfo();

            var rawJson = await Helper.QueryHelper.GetQuery<string>(Endpoint + string.Format(StatApi, symbol));
            var iexTradingStockModel = JsonConvert.DeserializeObject<IExTradingStockModel>(rawJson);

            stockInfo.Symbol = iexTradingStockModel.Symbol;
            stockInfo.LastTradePrice = iexTradingStockModel.LatestPrice;
            stockInfo.ChangePercentage = iexTradingStockModel.ChangePercent;
            stockInfo.CurrentVolume = iexTradingStockModel.LatestVolume;
            stockInfo.AvTotalVolume = iexTradingStockModel.AvgTotalVolume;
            stockInfo.Week52High = iexTradingStockModel.Week52High;
            stockInfo.Week52Low = iexTradingStockModel.Week52Low;
            stockInfo.DividendYield = iexTradingStockModel.DividendYield;

            return stockInfo;
        }

        private class IExTradingStockModel
        {
            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("latestPrice")]
            public decimal LatestPrice { get; set; }

            [JsonProperty("latestVolume")]
            public double LatestVolume { get; set; }

            [JsonProperty("changePercent")]
            public double ChangePercent { get; set; }

            [JsonProperty("avgTotalVolume")]
            public double AvgTotalVolume { get; set; }

            [JsonProperty("week52High")]
            public decimal Week52High { get; set; }

            [JsonProperty("week52Low")]
            public decimal Week52Low { get; set; }

            [JsonProperty("dividendYield")]
            public decimal DividendYield { get; set; }
        }
    }
}

