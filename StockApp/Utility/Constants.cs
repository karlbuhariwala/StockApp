using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Utility
{
    public class Constants
    {
        internal const string StockIdentityFileName = ".\\StocksOfInterest.txt";

        internal const string StockAnalysisFileName = ".\\Analysis.csv";

        internal static Dictionary<string, string> ExchangeMap = new Dictionary<string, string>()
        {
            { "NSE", "NASDAQ" },
        };
    }
}
