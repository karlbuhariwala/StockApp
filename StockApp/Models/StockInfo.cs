// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockInfo.cs

namespace StockApp.Models
{
    using System;

    public class StockInfo
    {
        public string Exchange { get; set; }

        public string Symbol { get; set; }

        public decimal? Ask { get; set; }

        public decimal? Bid { get; set; }

        public decimal? LastTradePrice { get; set; }

        public double ChangePercentage { get; set; }

        public decimal? DividendYield { get; set; }

        public DateTime ExDividendDate { get; set; }

        public DateTime EarningCallDate { get; set; }

        public double CurrentVolume { get; set; }

        public DateTimeOffset LastUpdate { get; internal set; }
    }
}
