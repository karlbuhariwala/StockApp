using RestServiceV1.Providers;
using StockApp.Provider.GoogleFinancePage;
using StockApp.Provider.GoogleStock;
using StockApp.Provider.YahooStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IYahooProvider provider = (IYahooProvider)ProviderFactory.Instance.CreateProvider<IYahooProvider>();
            var quote = provider.GetCurrentQuote("AAPL");

            IGoogleProvider googleProvider = (IGoogleProvider)ProviderFactory.Instance.CreateProvider<IGoogleProvider>();
            var quote2 = googleProvider.GetCurrentQuote("NSE:AAPL");

            IGoogleFinancePageProvider googlePageProvider = (IGoogleFinancePageProvider)ProviderFactory.Instance.CreateProvider<IGoogleFinancePageProvider>();
            var quote3 = googlePageProvider.GetCurrentVolume("NASDAQ", "AAPL");
            var vol1 = googlePageProvider.GetCurrentVolume("NYSE", "KOP");
        }
    }
}
