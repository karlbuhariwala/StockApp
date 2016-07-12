using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockApp.Models;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace StockApp.Provider.GoogleStock
{
    public class GoogleProvider : IGoogleProvider
    {
        private const string QueryFormat = @"http://finance.google.com/finance/info?client=ig&q={0}";

        Quote IGoogleProvider.GetCurrentQuote(string symbol)
        {
            var rawData = GoogleProvider.RunQuery<List<GoogleStock>>(string.Format(GoogleProvider.QueryFormat, symbol));
            var stock = rawData.FirstOrDefault();
            Quote quote = new Quote();
            quote.Symbol = stock.Symbol;
            quote.LastTradePrice = decimal.Parse(stock.CurrentPrice);

            return quote;
        }

        private static T RunQuery<T>(string query)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(query);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        // There is a // at the begining
                        for (int i = 0; i < 4; i++)
                        {
                            responseStream.ReadByte();
                        }

                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                        return (T)serializer.ReadObject(responseStream);
                    }
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
