using StockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider
{
    public interface ICurrentVolumeProvider : IProvider
    {
        Task<double> GetCurrentVolume(string exchange, string symbol);
    }
}
