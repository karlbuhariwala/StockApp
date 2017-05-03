// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockProvider.cs

namespace StockApp.Provider.StockProvider
{
    using Interfaces;
    using StockApp.Models;
    using StockApp.Provider.GoogleStock;
    using StockApp.Providers;
    using System.Threading.Tasks;

    public class StockProvider : IStockProvider
    {
        private readonly IGoogleProvider googleProvider;

        private readonly ICurrentVolumeProvider currentVolumeProvider;

        public StockProvider(IGoogleProvider googleProvider, ICurrentVolumeProvider currentVolumeProvider)
        {
            this.googleProvider = googleProvider;
            this.currentVolumeProvider = currentVolumeProvider;
        }

        public async Task<StockInfo> GetStockInfo(string exchange, string symbol)
        {
            StockInfo stockInfo = await this.googleProvider.GetCurrentQuote(exchange, symbol);
            stockInfo.CurrentVolume = await this.currentVolumeProvider.GetCurrentVolume(exchange, symbol);
            stockInfo.Exchange = exchange;

            return stockInfo;
        }
    }
}

