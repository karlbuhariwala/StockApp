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

    public class VolumeSpikeAnalyzer
    {
        private readonly IStockProvider stockProvider;

        private readonly IStorageProvider sqlProvider;

        private readonly List<StockIdentity> stocks;

        private readonly DateTimeOffset startDate;

        public VolumeSpikeAnalyzer(StockListGenerator stockGenerator, IStorageProvider sqlProvider, IStockProvider stockProvider)
        {
            this.stocks = stockGenerator.Stocks;
            this.stockProvider = stockProvider;
            this.sqlProvider = sqlProvider;
            this.startDate = DateTimeOffset.Now.AddDays(-3).Date;
            if (this.startDate.DayOfWeek == DayOfWeek.Sunday || this.startDate.DayOfWeek == DayOfWeek.Saturday)
            {
                this.startDate = this.startDate.AddDays(-2);
            }
        }

        public async Task RunPlugin()
        {
            foreach (var stock in this.stocks)
            {
                var stockInfo = await this.sqlProvider.GetStockPricesToProcess(stock.Symbol, this.startDate);
                // For each day
                // Find average volume throughout the day
                // Find range when it was more than average
                // Check if in that range it went up.

                var dates = stockInfo.Select(x => x.LastUpdate.Date).Distinct();

                foreach (var date in dates)
                {
                    bool result = GetAnalysisResult(stockInfo.Where(x => date < x.LastUpdate && x.LastUpdate < date.AddDays(1)));
                }
            }
        }

        private bool GetAnalysisResult(IEnumerable<StockInfo> enumerable)
        {
            var xAxisValues = enumerable.Select(x => (double)x.LastUpdate.Ticks).ToArray();
            var yAxisValues = enumerable.Select(x => (double)x.CurrentVolume).ToArray();

            double rSquared;
            double yIntercept;
            double slope;
            LinearRegression.Calculate(xAxisValues, yAxisValues, 0, enumerable.Count() - 1, out rSquared, out yIntercept, out slope);


            return false;
        }

        private class AnalysisResultContainer
        {
            public AnalysisResultContainer()
            {
            }

            public AnalysisResultContainer(DateTime start, DateTime end)
            {
                this.Start = start;
                this.End = end;
                this.FoundPattern = true;
            }

            public DateTime Start { get; set; }

            public DateTime End { get; set; }

            public bool FoundPattern { get; set; }
        }
    }
}
