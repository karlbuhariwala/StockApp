﻿using StockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider.GoogleStock
{
    public interface IGoogleProvider : IProvider
    {
        Quote GetCurrentQuote(string symbol);
    }
}