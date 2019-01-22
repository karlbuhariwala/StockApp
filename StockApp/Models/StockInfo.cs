// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockInfo.cs

namespace StockApp.Models
{
    using System;

    public class StockInfo
    {
        #region Basic
        public string Exchange { get; set; }

        public string Symbol { get; set; }
        #endregion

        #region Daily metrics
        public decimal? LastTradePrice { get; set; }

        public double ChangePercentage { get; set; }

        public double CurrentVolume { get; set; }
        #endregion

        #region Aggregated metrics
        public decimal? DividendYield { get; set; }

        public double AvTotalVolume { get; set; }

        public decimal? Week52High { get; set; }

        public decimal? Week52Low { get; set; }
        #endregion

        #region Events
        public DateTime ExDividendDate { get; set; }

        public DateTime EarningCallDate { get; set; }
        #endregion

        #region Advanced info
        public decimal? Ask { get; set; }

        public decimal? Bid { get; set; }
        #endregion

        public DateTimeOffset LastUpdate { get; internal set; }
    }
}
