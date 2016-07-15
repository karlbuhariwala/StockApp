using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider.YahooEarnings
{
    public interface IYahooEarningsProvider : IProvider
    {
        DateTime GetEarningsCallDate(string symbol);
    }
}
