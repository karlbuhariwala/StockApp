// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockScoreGenerator.cs

namespace StockApp.ServiceLayer
{
    using Interfaces;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class StockScoreGenerator
    {
        private readonly IStorageProvider sqlProvider;

        private readonly IStockProvider stockProvider;

        private readonly List<StockIdentityContainer> stocks;

        public StockScoreGenerator(StockListGenerator stockGenerator, IStorageProvider sqlProvider, IStockProvider stockProvider)
        {
            this.stocks = stockGenerator.Stocks;
            this.sqlProvider = sqlProvider;
            this.stockProvider = stockProvider;
        }

        public async Task RunPlugin()
        {
            foreach (var stock in this.stocks)
            {
                var stockInfo = await this.sqlProvider.GetLastStock(stock.Symbol);
            }
        }
    }
}
