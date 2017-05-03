// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = IGoogleProvider.cs

namespace StockApp.Provider.GoogleStock
{
    using StockApp.Models;
    using System.Threading.Tasks;

    public interface IGoogleProvider
    {
        Task<StockInfo> GetCurrentQuote(string exchange, string symbol);
    }
}
