using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Models
{
    public class StockProfile
    {
        public string Symbol { get; set; }

        public decimal? Ask { get; set; }

        public decimal? Bid { get; set; }

        public decimal? LastTradePrice { get; set; }

        public double ChangePercentage { get; set; }

        public DateTime ExDividendDate { get; set; }

        public DateTime EarningCallDate { get; set; }

        public double CurrentVolume { get; set; }

        public DateTime LastUpdate { get; internal set; }
    }
}
