// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = IStorageProvider.cs

namespace StockApp.Interfaces
{
    using StockApp.Models;
    using System.Threading.Tasks;

    public interface IStorageProvider
    {
        Task SaveStockInfo(StockInfo stockInfo);

        Task<StockInfo> GetLastStock(string symbol);
    }
}
