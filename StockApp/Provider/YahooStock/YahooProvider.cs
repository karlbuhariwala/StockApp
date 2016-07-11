using StockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StockApp.Provider.YahooStock
{
    public class YahooProvider : IYahooProvider
    {
        private const string currentQuoteQuery = @"http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(""{0}"")&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
        Quote IYahooProvider.GetCurrentQuote(string symbol)
        {
            XDocument document = XDocument.Load(string.Format(currentQuoteQuery, symbol));
            return YahooProvider.Parse(document, symbol);
        }

        private static Quote Parse(XDocument document, string symbol)
        {
            //// Resource http://www.jarloo.com/get-yahoo-finance-api-data-via-yql/
            Quote quote = new Quote();

            XElement results = document.Root.Element("results");
            XElement q = results.Elements("quote").First(w => w.Attribute("symbol").Value == symbol);

            quote.Ask = YahooProvider.GetDecimal(q.Element("Ask").Value);
            quote.Bid = YahooProvider.GetDecimal(q.Element("Bid").Value);
            //quote.AverageDailyVolume = GetDecimal(q.Element("AverageDailyVolume").Value);
            //quote.BookValue = GetDecimal(q.Element("BookValue").Value);
            //quote.Change = GetDecimal(q.Element("Change").Value);
            //quote.DividendShare = GetDecimal(q.Element("DividendShare").Value);
            //quote.LastTradeDate = GetDateTime(q.Element("LastTradeDate").Value + " " + q.Element("LastTradeTime").Value);
            //quote.EarningsShare = GetDecimal(q.Element("EarningsShare").Value);
            //quote.EpsEstimateCurrentYear = GetDecimal(q.Element("EPSEstimateCurrentYear").Value);
            //quote.EpsEstimateNextYear = GetDecimal(q.Element("EPSEstimateNextYear").Value);
            //quote.EpsEstimateNextQuarter = GetDecimal(q.Element("EPSEstimateNextQuarter").Value);
            //quote.DailyLow = GetDecimal(q.Element("DaysLow").Value);
            //quote.DailyHigh = GetDecimal(q.Element("DaysHigh").Value);
            //quote.YearlyLow = GetDecimal(q.Element("YearLow").Value);
            //quote.YearlyHigh = GetDecimal(q.Element("YearHigh").Value);
            //quote.MarketCapitalization = GetDecimal(q.Element("MarketCapitalization").Value);
            //quote.Ebitda = GetDecimal(q.Element("EBITDA").Value);
            //quote.ChangeFromYearLow = GetDecimal(q.Element("ChangeFromYearLow").Value);
            //quote.PercentChangeFromYearLow = GetDecimal(q.Element("PercentChangeFromYearLow").Value);
            //quote.ChangeFromYearHigh = GetDecimal(q.Element("ChangeFromYearHigh").Value);
            quote.LastTradePrice = YahooProvider.GetDecimal(q.Element("LastTradePriceOnly").Value);
            //quote.PercentChangeFromYearHigh = GetDecimal(q.Element("PercebtChangeFromYearHigh").Value); //missspelling in yahoo for field name
            //quote.FiftyDayMovingAverage = GetDecimal(q.Element("FiftydayMovingAverage").Value);
            //quote.TwoHunderedDayMovingAverage = GetDecimal(q.Element("TwoHundreddayMovingAverage").Value);
            //quote.ChangeFromTwoHundredDayMovingAverage = GetDecimal(q.Element("ChangeFromTwoHundreddayMovingAverage").Value);
            //quote.PercentChangeFromTwoHundredDayMovingAverage = GetDecimal(q.Element("PercentChangeFromTwoHundreddayMovingAverage").Value);
            //quote.PercentChangeFromFiftyDayMovingAverage = GetDecimal(q.Element("PercentChangeFromFiftydayMovingAverage").Value);
            //quote.Name = q.Element("Name").Value;
            //quote.Open = GetDecimal(q.Element("Open").Value);
            //quote.PreviousClose = GetDecimal(q.Element("PreviousClose").Value);
            //quote.ChangeInPercent = GetDecimal(q.Element("ChangeinPercent").Value);
            //quote.PriceSales = GetDecimal(q.Element("PriceSales").Value);
            //quote.PriceBook = GetDecimal(q.Element("PriceBook").Value);
            //quote.ExDividendDate = GetDateTime(q.Element("ExDividendDate").Value);
            //quote.PeRatio = GetDecimal(q.Element("PERatio").Value);
            //quote.DividendPayDate = GetDateTime(q.Element("DividendPayDate").Value);
            //quote.PegRatio = GetDecimal(q.Element("PEGRatio").Value);
            //quote.PriceEpsEstimateCurrentYear = GetDecimal(q.Element("PriceEPSEstimateCurrentYear").Value);
            //quote.PriceEpsEstimateNextYear = GetDecimal(q.Element("PriceEPSEstimateNextYear").Value);
            //quote.ShortRatio = GetDecimal(q.Element("ShortRatio").Value);
            //quote.OneYearPriceTarget = GetDecimal(q.Element("OneyrTargetPrice").Value);
            //quote.Volume = GetDecimal(q.Element("Volume").Value);
            //quote.StockExchange = q.Element("StockExchange").Value;

            quote.LastUpdate = DateTime.Now;

            return quote;
        }

        private static decimal? GetDecimal(string input)
        {
            input = !string.IsNullOrEmpty(input) ? input.Replace("%", "") : null;
            decimal value;

            if (!string.IsNullOrEmpty(input) && Decimal.TryParse(input, out value))
            {
                return value;
            }

            return null;
        }

        private static DateTime? GetDateTime(string input)
        {
            DateTime value;
            if (!string.IsNullOrEmpty(input) && DateTime.TryParse(input, out value))
            {
                return value;
            }

            return null;
        }
    }
}

