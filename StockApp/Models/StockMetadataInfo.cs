// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockMetadataInfo.cs

namespace StockApp.Models
{
    using System;

    public class StockMetadataInfo
    {
        public string Exchange { get; set; }

        public string Symbol { get; set; }

        public DateTimeOffset LastProcessedDateTime { get; set; }

        public double PercentageChangeRange { get; set; }

        public int DataPointCount { get; set; }

        public DateTimeOffset LastUpdate { get; internal set; }
    }
}
