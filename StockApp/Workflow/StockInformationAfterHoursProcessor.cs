// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockInformationCollector.cs

namespace StockApp.ServiceLayer
{
    using System;
    using System.Threading.Tasks;

    public class StockInformationAfterHoursProcessor
    {
        private readonly StockScoreGenerator stockScoreGenerator;

        private readonly TradingHoursChecker tradingHoursChecker;

        private readonly StockRangeGenerator stockRangeGenerator;

        public StockInformationAfterHoursProcessor(StockScoreGenerator stockScoreGenerator, TradingHoursChecker tradingHoursChecker, StockRangeGenerator stockRangeGenerator)
        {
            this.stockScoreGenerator = stockScoreGenerator;
            this.tradingHoursChecker = tradingHoursChecker;
            this.stockRangeGenerator = stockRangeGenerator;
        }

        public async Task DoWork(bool force = false)
        {
            if (!this.tradingHoursChecker.IsTradingHours() || force)
            {
                await this.stockRangeGenerator.UpdateRangesInfo();
            }
        }
    }
}
