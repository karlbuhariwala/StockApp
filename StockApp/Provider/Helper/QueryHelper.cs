using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace StockApp.Provider.Helper
{
    public static class QueryHelper
    {
        public async static Task<T> GetQuery<T>(string query, int bytesToSkip = 0)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(query);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        // There is a // at the begining
                        for (int i = 0; i < bytesToSkip; i++)
                        {
                            responseStream.ReadByte();
                        }

                        if (typeof(T) == typeof(string))
                        {
                            StreamReader streamReader = new StreamReader(responseStream);
                            return (T)Convert.ChangeType(streamReader.ReadToEnd(), typeof(T));
                        }
                        else
                        {
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                            return (T)serializer.ReadObject(responseStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException && (int)((ex as WebException).Response as HttpWebResponse).StatusCode == 429)
                {

                }
                Console.WriteLine("Exception " + query);

                throw new ApplicationException("Exception with query: " + query, ex);
            }
        }
    }
}
