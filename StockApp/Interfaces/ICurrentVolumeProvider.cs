// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = ICurrentVolumeProvider.cs

namespace StockApp.Interfaces
{
    using System.Threading.Tasks;

    public interface ICurrentVolumeProvider
    {
        Task<double> GetCurrentVolume(string exchange, string symbol);
    }
}
