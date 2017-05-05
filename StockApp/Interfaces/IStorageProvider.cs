// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = IStorageProvider.cs

namespace StockApp.Interfaces
{
    using StockApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStorageProvider
    {
        Task SaveStockInfo(StockInfo stockInfo);

        Task<StockInfo> GetLastStock(string symbol);

        Task<StockScore> GetLastStockScore(string symbol);

        Task<List<StockInfo>> GetStockPricesToProcess(string symbol, DateTimeOffset dateTime);

        Task<Dictionary<string, double>> GetStockRanges();

        Task BulkInsertScores(List<StockScore> stockScores);
    }
}
