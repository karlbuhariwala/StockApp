using StockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider.YahooStock
{
    public interface IYahooProvider : IProvider
    {
        Quote GetCurrentQuote(string symbol);
    }
}
