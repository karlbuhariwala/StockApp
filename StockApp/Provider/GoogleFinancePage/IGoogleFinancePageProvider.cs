﻿using StockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider.GoogleFinancePage
{
    public interface IGoogleFinancePageProvider : IProvider
    {
        double GetCurrentVolume(string exchange, string symbol);
    }
}