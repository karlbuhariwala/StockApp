﻿using RestServiceV1.Providers;
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
        }
    }
}