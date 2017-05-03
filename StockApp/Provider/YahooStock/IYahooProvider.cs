// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = IYahooProvider.cs

namespace StockApp.Provider.YahooStock
{
    using StockApp.Models;
    using System.Threading.Tasks;

    public interface IYahooProvider
    {
        Task<StockInfo> GetCurrentQuote(string symbol);
    }
}
