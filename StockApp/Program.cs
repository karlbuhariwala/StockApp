using Newtonsoft.Json;
using RestServiceV1.Providers;
using StockApp.Models;
using StockApp.Provider;
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

            List<Task<string>> listOfTasks = new List<Task<string>>();
            StringBuilder sb = new StringBuilder();
            sb.Append("Name,LastTradePrice,ChangePercentage,CurrentVolume,ExDividendDate,EarningDate,LastUpdated,Comment" + Environment.NewLine);
            foreach (var stock in listOfStock)
            {
                listOfTasks.Add(Task.Run<string>(() => Program.GetStockInfo(stock)));
            }

            Task.WaitAll(Task.WhenAll(listOfTasks));

            foreach (var task in listOfTasks)
            {
                // Check if completed
                sb.Append(task.Result + Environment.NewLine);
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

            try
            {
                if (!Constants.ExchangeMap.ContainsKey(stock.Exchange))
                {
                    throw new ApplicationException("Exchange key cannot be mapped. Key=" + stock.Exchange);
                }

                IGoogleProvider googleProvider = ProviderFactory.Instance.CreateProvider<IGoogleProvider>();
                ICurrentVolumeProvider googlePageProvider = ProviderFactory.Instance.CreateProvider<ICurrentVolumeProvider>();
                IExDividendDateProvider exDividendDateProvider = ProviderFactory.Instance.CreateProvider<IExDividendDateProvider>();
                IEarningsDateProvider earningProvider = ProviderFactory.Instance.CreateProvider<IEarningsDateProvider>();

                var currentQuoteTask = Retry.Do<StockProfile>(() => googleProvider.GetCurrentQuote(stock.Exchange, stock.Symbol), TimeSpan.FromSeconds(2));
                var currentVolumeTask = Retry.Do<double>(() => googlePageProvider.GetCurrentVolume(Constants.ExchangeMap[stock.Exchange], stock.Symbol), TimeSpan.FromSeconds(2));
                var exDividendDateTask = Retry.Do<DateTime>(() => exDividendDateProvider.GetExDividendDate(stock.Symbol.ToLower()), TimeSpan.FromSeconds(2));
                var earningCallDateTask = Retry.Do<DateTime>(() => earningProvider.GetEarningsCallDate(stock.Symbol.ToLower()), TimeSpan.FromSeconds(2));

                Task.WhenAll(currentQuoteTask, currentVolumeTask, exDividendDateTask, earningCallDateTask);

                // Check if all completed

                StockProfile stockProfile = currentQuoteTask.Result;
                stockProfile.CurrentVolume = currentVolumeTask.Result;
                stockProfile.ExDividendDate = exDividendDateTask.Result;
                stockProfile.EarningCallDate = earningCallDateTask.Result;

                List<string> listToPrint = new List<string>()
                {
                    stock.Exchange + ":" + stock.Symbol,
                    stockProfile.LastTradePrice.ToString(),
                    stockProfile.ChangePercentage.ToString(),
                    stockProfile.CurrentVolume.ToString(),
                    stockProfile.ExDividendDate.ToString(),
                    stockProfile.EarningCallDate.ToString(),
                    stockProfile.LastUpdate.ToString(),
                    string.Empty, // Comment
                };

                return string.Join(",", listToPrint);
            }
            catch (Exception)
            {
                return stock.Exchange + ":" + stock.Symbol + "," + "Error";
            }
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
