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

        public StockRangeGenerator(StockListGenerator stockGenerator, IStorageProvider sqlProvider)
        {
            this.stocks = stockGenerator.Stocks;
            this.sqlProvider = sqlProvider;

            var task = this.sqlProvider.GetStockRanges();
            task.Wait();
            this.StockRanges = task.Result;
        }

        public Dictionary<string, double> StockRanges { get; set; }
    }
}
