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
        private readonly LiveStockInfoCollector liveStockInfoCollector;

        private readonly StockScoreGenerator stockScoreGenerator;

        private TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public StockInformationCollector(LiveStockInfoCollector liveStockInfoCollector, StockScoreGenerator stockScoreGenerator)
        {
            this.liveStockInfoCollector = liveStockInfoCollector;
            this.stockScoreGenerator = stockScoreGenerator;
        }

        public async Task DoWork(bool force = false)
        {
            while (true)
            {
                var easternNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, est);
                if ((TimeSpan.FromHours(9) < easternNow.TimeOfDay && easternNow.TimeOfDay < TimeSpan.FromHours(16)) || force)
                {
                    Console.WriteLine(DateTime.UtcNow.ToString() + ": Running now...");
                    await this.liveStockInfoCollector.RunPlugin();
                    Console.WriteLine(DateTime.UtcNow.ToString() + ": Completed running now...");
                }

                Console.WriteLine(DateTime.UtcNow.ToString() + ": Sleeping now...");
                await Task.Delay(5 * 60 * 1000);
                Console.WriteLine(DateTime.UtcNow.ToString() + ": Awake now...");
            }

            ////await this.stockScoreGenerator.RunPlugin();
        }
    }
}
