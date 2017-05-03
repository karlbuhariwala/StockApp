// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockListGenerator.cs

namespace StockApp.ServiceLayer
{
    using StockApp.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using Utility;

    public class LiveStockInfoCollector
    {
        private readonly IStockProvider stockProvider;

        private readonly IStorageProvider sqlProvider;

        private readonly List<StockIdentityContainer> stocks;

        public LiveStockInfoCollector(StockListGenerator stockGenerator, IStorageProvider sqlProvider, IStockProvider stockProvider)
        {
            this.stocks = stockGenerator.Stocks;
            this.stockProvider = stockProvider;
            this.sqlProvider = sqlProvider;
        }

        public async Task RunPlugin()
        {
            var stockInfo = new List<StockInfo>();
            foreach (var stock in this.stocks)
            {
                stockInfo.Add(await Retry.Do<StockInfo>(() => this.stockProvider.GetStockInfo(stock.Exchange, stock.Symbol), TimeSpan.FromSeconds(2)));
            }

            foreach (var stock in stockInfo)
            {
                await Retry.Do(() => this.sqlProvider.SaveStockInfo(stock), TimeSpan.FromSeconds(2));
            }
        }
    }
}
