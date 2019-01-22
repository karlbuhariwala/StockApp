// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockScoreGenerator.cs

namespace StockApp.ServiceLayer
{
    using Interfaces;
    using Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class StockScoreGenerator
    {
        private readonly IStorageProvider sqlProvider;

        private readonly List<StockIdentity> stocks;

        private readonly Dictionary<string, double> stockRages;

        private static DateTime defaultDateTime = new DateTime(0001, 1, 1);

        public StockScoreGenerator(StockListGenerator stockListGenerator, StockRangeGenerator stockRangeGenerator, IStorageProvider sqlProvider)
        {
            this.stocks = stockListGenerator.Stocks;
            this.stockRages = stockRangeGenerator.StockRanges;
            this.sqlProvider = sqlProvider;
        }

        public async Task RunPlugin(TimeSpan timeInterval)
        {
            // for each stock... Get last run from table... Then get raw from table... start from begining time range and add ... accumulate and store back.
            foreach (var stock in this.stocks)
            {
                // timeInterval should be passed in here to help determine which table to go to.
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

                int i = 0;
                while (i < stockPrices.Count)
                {
                    double startChangePercentage = stockPrices[i].ChangePercentage;
                    DateTimeOffset startDateTime = stockPrices[i].LastUpdate.Add(timeInterval);
                    var stocksPricesToProcess = stockPrices.Skip(i).Where(x => x.LastUpdate.Date == startDateTime.Date).ToList();
                    i += stocksPricesToProcess.Count;
                    GenerateScores(timeInterval, startDateTime, stocksPricesToProcess, startChangePercentage, increment, stockScores); 
                }

                await this.sqlProvider.BulkInsertScores(stockScores);
            }
        }

        private static void GenerateScores(TimeSpan timeInterval, DateTimeOffset startDateTime, List<StockInfo> stockPrices, double startChangePercentage, double increment, List<StockScore> stockScores)
        {
            DateTimeOffset dateTimeBatchEnd = startDateTime;
            for (int index = 0; index < stockPrices.Count; dateTimeBatchEnd = dateTimeBatchEnd.Add(timeInterval))
            {
                double sumOfPercentageChange = 0;
                int countOfPercentageChange = 0;
                while (index < stockPrices.Count && stockPrices[index].LastUpdate < dateTimeBatchEnd)
                {
                    sumOfPercentageChange += stockPrices[index].ChangePercentage;
                    countOfPercentageChange += 1;
                    index += 1;
                }

                double averagePercentageChange = sumOfPercentageChange / countOfPercentageChange;

                double diff = averagePercentageChange - startChangePercentage;
                int diffScore = (int)(diff / increment);
                if (diffScore > 4)
                {
                    diffScore = 4;
                }
                else if (diffScore < -5)
                {
                    diffScore = -5;
                }

                stockScores.Add(new StockScore() { LastUpdate = dateTimeBatchEnd, Score = 5 + diffScore, Symbol = stockPrices[0].Symbol });
            }
        }
    }
}
