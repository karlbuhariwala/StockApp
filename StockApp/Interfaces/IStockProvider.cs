// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = IStockProvider.cs

namespace StockApp.Interfaces
{
    using StockApp.Models;
    using System.Threading.Tasks;

    public interface IStockProvider
    {
        Task<StockInfo> GetStockInfo(string exchange, string symbol);
    }
}
