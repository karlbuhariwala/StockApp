using Newtonsoft.Json;
using RestServiceV1.Providers;
using StockApp.Models;
using StockApp.Provider.DiviData;
using StockApp.Provider.GoogleFinancePage;
using StockApp.Provider.GoogleStock;
using StockApp.Provider.YahooEarnings;
using StockApp.Provider.YahooStock;
using StockApp.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                switch (args[0])
                {
                    case "AddStock":
                        Program.AddStock();
                        break;
                    case "RunAnalysis":
                        Program.RunAnalysis();
                        break;
                    case "/?":
                    case "-?":
                        Console.WriteLine("Usage is StockApp.exe CommandName");
                        Console.WriteLine("Commands are:");
                        Console.WriteLine("AddStock");
                        break;
                    default:
                        break;
                }
            }

            return;
        }

        private static void RunAnalysis()
        {
            string jsonText = File.ReadAllText(Constants.StockIdentityFileName);
            var listOfStock = JsonConvert.DeserializeObject<List<StockIdentityContainer>>(jsonText);

            StringBuilder sb = new StringBuilder();
            sb.Append("Name,LastTradePrice,CurrentVolume,ExDividendDate,EarningDate" + Environment.NewLine);
            foreach (var stock in listOfStock)
            {
                sb.Append(Program.GetStockInfo(stock) + Environment.NewLine);
            }

            File.Create(Constants.StockAnalysisFileName).Close();
            File.AppendAllText(Constants.StockAnalysisFileName, sb.ToString());
        }

        private static string GetStockInfo(StockIdentityContainer stock)
        {
            //string stockSymbol = "MSFT";
            //string exchange = "NSE";
            //string exchangeAlt = "NASDAQ";

            //IYahooProvider provider = ProviderFactory.Instance.CreateProvider<IYahooProvider>();
            //var quote = provider.GetCurrentQuote("AAPL");

            if (!Constants.ExchangeMap.ContainsKey(stock.Exchange))
            {
                throw new ApplicationException("Exchange key cannot be mapped. Key=" + stock.Exchange);
            }

            IGoogleProvider googleProvider = ProviderFactory.Instance.CreateProvider<IGoogleProvider>();
            var stockProfile = googleProvider.GetCurrentQuote(stock.Exchange, stock.Symbol);

            IGoogleFinancePageProvider googlePageProvider = ProviderFactory.Instance.CreateProvider<IGoogleFinancePageProvider>();
            stockProfile.CurrentVolume = googlePageProvider.GetCurrentVolume(Constants.ExchangeMap[stock.Exchange], stock.Symbol);

            IDiviDataProvider streetProvider = ProviderFactory.Instance.CreateProvider<IDiviDataProvider>();
            stockProfile.ExDividendDate = streetProvider.GetExDividendDate(stock.Symbol);

            IYahooEarningsProvider earningProvider = ProviderFactory.Instance.CreateProvider<IYahooEarningsProvider>();
            stockProfile.EarningCallDate = earningProvider.GetEarningsCallDate(stock.Symbol.ToLower());

            return stock.Exchange + ":" + stock.Symbol + "," + stockProfile.LastTradePrice + "," + stockProfile.CurrentVolume + "," + stockProfile.ExDividendDate + "," + stockProfile.EarningCallDate;
        }

        private static void AddStock()
        {
            Console.WriteLine("Stock Symbol:");
            string symbol = Console.ReadLine();
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ApplicationException("No symbol provided");
            }

            Console.WriteLine("Stock exchange:");
            string exchange = Console.ReadLine();
            if (string.IsNullOrEmpty(exchange))
            {
                throw new ApplicationException("No exchange provided");
            }

            StockIdentityContainer container = new StockIdentityContainer()
            {
                Symbol = symbol,
                Exchange = exchange,
            };

            string jsonText = File.ReadAllText(Constants.StockIdentityFileName);
            var listOfStock = JsonConvert.DeserializeObject<List<StockIdentityContainer>>(jsonText);
            if (!listOfStock.Contains(container, new StockIdentityComparer()))
            {
                listOfStock.Add(container);
            }

            File.WriteAllText(Constants.StockIdentityFileName + ".old", jsonText);
            File.Create(Constants.StockIdentityFileName).Close();
            File.WriteAllText(Constants.StockIdentityFileName, JsonConvert.SerializeObject(listOfStock));
        }
    }
}
