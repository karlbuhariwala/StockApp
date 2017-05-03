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

        public StockInformationCollector(LiveStockInfoCollector liveStockInfoCollector, StockScoreGenerator stockScoreGenerator)
        {
            this.liveStockInfoCollector = liveStockInfoCollector;
            this.stockScoreGenerator = stockScoreGenerator;
        }

        public async Task DoWork()
        {
            await this.liveStockInfoCollector.RunPlugin();
            await this.stockScoreGenerator.RunPlugin();
        }
    }
}
