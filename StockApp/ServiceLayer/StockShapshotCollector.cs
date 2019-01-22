// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockRangeGenerator.cs

namespace StockApp.ServiceLayer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using StockApp.Interfaces;

    public class StockShapshotCollector
    {
        private readonly List<StockIdentity> stocks;

        private readonly IEarningsDateProvider earningsDateProvider;

        private readonly IStockProvider stockProvider;

        public StockShapshotCollector(StockListGenerator stockGenerator, IEarningsDateProvider earningsDateProvider, IStockProvider stockProvider)
        {
            this.stocks = stockGenerator.Stocks;
            this.earningsDateProvider = earningsDateProvider;
            this.stockProvider = stockProvider;
        }

        public async Task CreateShapshot(string path)
        {
            File.AppendAllText(path, $@"Symbol,LastTradePrice,ChangePercentage,DividendYield,CurrentVolume,AvTotalVolume,Week52High,Week52Low,EarningDate" + Environment.NewLine);
            var results = new List<StockInfo>();
            foreach (var stock in this.stocks)
            {
                var stockInfo = new StockInfo();
                stockInfo.Symbol = stock.Symbol;

                try
                {
                    stockInfo = await this.stockProvider.GetStockInfo(stock.Exchange, stock.Symbol);
                }
                catch (Exception)
                {
                }
                finally
                {
                    results.Add(stockInfo);
                }

                try
                {
                    stockInfo.EarningCallDate = await this.earningsDateProvider.GetEarningsCallDate(stock.Symbol);
                }
                catch (Exception)
                {
                }
            }

            foreach (var result in results)
            {
                var earningDate = result.EarningCallDate != null ? result.EarningCallDate.ToString() : string.Empty;
                File.AppendAllText(path, $@"{result.Symbol},{result.LastTradePrice},{result.ChangePercentage},{result.DividendYield},{result.CurrentVolume},{result.AvTotalVolume},{result.Week52High},{result.Week52Low},{earningDate}" + Environment.NewLine);
            }
        }
    }
}
