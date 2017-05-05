// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockScoreGenerator.cs

namespace StockApp.ServiceLayer
{
    using Interfaces;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class StockScoreGenerator
    {
        private readonly IStorageProvider sqlProvider;

        private readonly IStockProvider stockProvider;

        private readonly List<StockIdentity> stocks;

        private readonly Dictionary<string, double> stockRages;

        private static DateTime defaultDateTime = new DateTime(0001, 1, 1);

        public StockScoreGenerator(StockListGenerator stockGenerator, StockRangeGenerator stockRangeGenerator, IStorageProvider sqlProvider, IStockProvider stockProvider)
        {
            this.stocks = stockGenerator.Stocks;
            this.stockRages = stockRangeGenerator.StockRanges;
            this.sqlProvider = sqlProvider;
            this.stockProvider = stockProvider;
        }

        public async Task RunPlugin()
        {
            foreach (var stock in this.stocks)
            {
                var lastStockScore = await this.sqlProvider.GetLastStockScore(stock.Symbol);

                DateTimeOffset dateToUpdateAfter = new DateTime(2017, 1, 1);
                if (lastStockScore.LastUpdate != defaultDateTime)
                {
                    dateToUpdateAfter = lastStockScore.LastUpdate;
                }

                List<StockInfo> stockPrices = await this.sqlProvider.GetStockPricesToProcess(stock.Symbol, dateToUpdateAfter);
                double range;
                this.stockRages.TryGetValue(stock.Symbol, out range);
                double increment = range / 10;

                List<StockScore> stockScores = new List<StockScore>();
                stockScores.Add(new StockScore() { LastUpdate = stockPrices[0].LastUpdate, Score = 5, Symbol = stock.Symbol });
                for (int i = 1; i < stockPrices.Count; i++)
                {
                    double diff = stockPrices[i].ChangePercentage - stockPrices[i - 1].ChangePercentage;
                    int diffScore = (int) (diff / increment);
                    if (diffScore > 4)
                    {
                        diffScore = 4;
                    }
                    else if (diffScore < -5)
                    {
                        diffScore = -5;
                    }

                    stockScores.Add(new StockScore() { LastUpdate = stockPrices[i].LastUpdate, Score = 5 + diffScore, Symbol = stock.Symbol });
                }

                await this.sqlProvider.BulkInsertScores(stockScores);
            }
        }
    }
}
