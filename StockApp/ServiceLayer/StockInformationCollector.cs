// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockInformationCollector.cs

namespace StockApp.ServiceLayer
{
    using Interfaces;
    using Newtonsoft.Json;
    using StockApp.Models;
    using StockApp.Utility;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class StockInformationCollector
    {
        private readonly List<StockIdentityContainer> listOfStock;

        private readonly IStockProvider stockProvider;

        private readonly IStorageProvider sqlProvider;

        public StockInformationCollector(IStorageProvider sqlProvider, IStockProvider stockProvider)
        {
            string jsonText = File.ReadAllText(Constants.StockIdentityFileName);
            this.listOfStock = JsonConvert.DeserializeObject<List<StockIdentityContainer>>(jsonText);

            this.stockProvider = stockProvider;
            this.sqlProvider = sqlProvider;
        }

        public async Task<StockInfo> DoWork()
        {
            var stockInfo = new List<StockInfo>();
            foreach (var stock in this.listOfStock)
            {
                stockInfo.Add(await Retry.Do<StockInfo>(() => this.stockProvider.GetStockInfo(stock.Exchange, stock.Symbol), TimeSpan.FromSeconds(2)));
            }

            foreach (var stock in stockInfo)
            {
                await Retry.Do<string>(() => this.sqlProvider.SaveStockInfo(stock), TimeSpan.FromSeconds(2));
            }

            return null;
        }
    }
}
