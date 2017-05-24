// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockRangeGenerator.cs

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

    public class StockRangeGenerator
    {
        private readonly IStorageProvider sqlProvider;

        private readonly List<StockIdentity> stocks;

        private readonly Dictionary<string, StockMetadataInfo> stockRangeInfo;

        public StockRangeGenerator(StockListGenerator stockGenerator, IStorageProvider sqlProvider)
        {
            this.stocks = stockGenerator.Stocks;
            this.sqlProvider = sqlProvider;

            var task = this.sqlProvider.GetStockRanges();
            task.Wait();

            this.StockRanges = new Dictionary<string, double>();
            foreach (var item in task.Result)
            {
                this.StockRanges.Add(item.Key, item.Value.PercentageChangeRange);
            }

            this.stockRangeInfo = task.Result;
        }

        public Dictionary<string, double> StockRanges { get; set; }

        public async Task UpdateRangesInfo()
        {
            foreach (var stock in this.stocks)
            {
                StockMetadataInfo stockInfo;
                this.stockRangeInfo.TryGetValue(stock.Symbol, out stockInfo);
                var output = await this.sqlProvider.GetRangesSinceLastProcessedDay(stock.Symbol, stockInfo?.LastProcessedDateTime.Date.AddDays(1) ?? new DateTime(2017, 1, 1));

                if (output.Item1.Any())
                {
                    int count = stockInfo?.DataPointCount ?? 0;
                    double rangeAccumulator = stockInfo?.PercentageChangeRange != null ? stockInfo.PercentageChangeRange * count : 0;
                    foreach (var range in output.Item1)
                    {
                        count += 1;
                        rangeAccumulator += range;
                    }

                    await this.sqlProvider.SaveRange(stock.Symbol, rangeAccumulator / count, count, output.Item2);
                }
            }
        }
    }
}
