// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockInformationCollector.cs

namespace StockApp.ServiceLayer
{
    using System;
    using System.Threading.Tasks;

    public class StockInformationCollector
    {
        private readonly LiveStockInfoCollector liveStockInfoCollector;

        private readonly TradingHoursChecker tradingHoursChecker;

        public StockInformationCollector(LiveStockInfoCollector liveStockInfoCollector, TradingHoursChecker tradingHoursChecker)
        {
            this.liveStockInfoCollector = liveStockInfoCollector;
            this.tradingHoursChecker = tradingHoursChecker;
        }

        public async Task DoWork(bool force = false)
        {
            while (true)
            {
                if (this.tradingHoursChecker.IsTradingHours() || force)
                {
                    Console.WriteLine(DateTime.UtcNow.ToString() + ": Running now...");
                    await this.liveStockInfoCollector.RunPlugin();
                    Console.WriteLine(DateTime.UtcNow.ToString() + ": Completed running now...");
                }

                Console.WriteLine(DateTime.UtcNow.ToString() + ": Sleeping now...");
                await Task.Delay(5 * 60 * 1000);
                Console.WriteLine(DateTime.UtcNow.ToString() + ": Awake now...");
            }
        }
    }
}
