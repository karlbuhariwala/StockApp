// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = IExDividendDateProvider.cs

namespace StockApp.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IExDividendDateProvider 
    {
        Task<DateTime> GetExDividendDate(string symbol);
    }
}
