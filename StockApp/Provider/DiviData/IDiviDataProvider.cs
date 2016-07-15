using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider.DiviData
{
    public interface IDiviDataProvider : IProvider
    {
        DateTime GetExDividendDate(string symbol);
    }
}
