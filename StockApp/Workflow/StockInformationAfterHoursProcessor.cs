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

        public StockInformationAfterHoursProcessor(StockScoreGenerator stockScoreGenerator, TradingHoursChecker tradingHoursChecker)
        {
            this.stockScoreGenerator = stockScoreGenerator;
            this.tradingHoursChecker = tradingHoursChecker;
        }

        public async Task DoWork(bool force = false)
        {
            if (!this.tradingHoursChecker.IsTradingHours() || force)
            {
                await this.stockScoreGenerator.RunPlugin();
            }
        }
    }
}
