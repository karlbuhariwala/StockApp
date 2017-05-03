// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = SqlProvider.cs

namespace StockApp.Provider.SqlProvider
{
    using StockApp.Interfaces;
    using StockApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class SqlProvider : IStorageProvider
    {
        private readonly string connectionString;

        public SqlProvider()
        {
            this.connectionString = ConfigurationManager.AppSettings["SqlStore"];
        }

        public async Task<string> SaveStockInfo(StockInfo stockInfo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@symbol", stockInfo.Symbol);
            parameters.Add("@timestamp", stockInfo.LastUpdate);
            parameters.Add("@price", stockInfo.LastTradePrice);
            parameters.Add("@volume", stockInfo.CurrentVolume);

            await this.RunQuery(SqlQuery.InsertRawData, parameters);

            return null;
        }

        private async Task RunQuery(string query, Dictionary<string, object> parameters)
        {
            SqlConnection sqlconnection = new SqlConnection(this.connectionString);
            try
            {
                SqlCommand myCommand = new SqlCommand(query, sqlconnection);
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> item in parameters)
                    {
                        myCommand.Parameters.AddWithValue(item.Key, item.Value ?? DBNull.Value);
                    }
                }

                myCommand.Connection.Open();
                await myCommand.ExecuteNonQueryAsync();
            }
            catch
            {
            }
        }
    }
}

