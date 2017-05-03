// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = IEarningsDateProvider.cs

namespace StockApp.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IEarningsDateProvider
    {
        Task<DateTime> GetEarningsCallDate(string symbol);
    }
}
