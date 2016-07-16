using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider.YahooEarnings
{
    public interface IYahooEarningsProvider : IProvider
    {
        Task<DateTime> GetEarningsCallDate(string symbol);
    }
}
