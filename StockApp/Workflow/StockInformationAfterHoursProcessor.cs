// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockInformationCollector.cs

namespace StockApp.ServiceLayer
{
    using System;
    using System.Threading.Tasks;

    public class StockInformationAfterHoursProcessor
    {
        private readonly VolumeSpikeAnalyzer volumeSpikeAnalyzer;

        private readonly TradingHoursChecker tradingHoursChecker;

        private readonly StockRangeGenerator stockRangeGenerator;

        public StockInformationAfterHoursProcessor(VolumeSpikeAnalyzer volumeSpikeAnalyzer, TradingHoursChecker tradingHoursChecker, StockRangeGenerator stockRangeGenerator)
        {
            this.volumeSpikeAnalyzer = volumeSpikeAnalyzer;
            this.tradingHoursChecker = tradingHoursChecker;
            this.stockRangeGenerator = stockRangeGenerator;
        }

        public async Task DoWork(bool force = false)
        {
            if (!this.tradingHoursChecker.IsTradingHours() || force)
            {
                //await this.stockRangeGenerator.UpdateRangesInfo();
                await this.volumeSpikeAnalyzer.RunPlugin();
            }
        }
    }
}
