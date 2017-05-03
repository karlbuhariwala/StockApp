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
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class SqlProvider : IStorageProvider
    {
        private readonly string connectionString;

        public SqlProvider()
        {
            this.connectionString = ConfigurationManager.AppSettings["SqlStore"];
        }

        public async Task SaveStockInfo(StockInfo stockInfo)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@symbol", stockInfo.Symbol);
            parameters.Add("@timestamp", stockInfo.LastUpdate);
            parameters.Add("@price", stockInfo.LastTradePrice);
            parameters.Add("@volume", stockInfo.CurrentVolume);
            parameters.Add("@changePercentage", stockInfo.ChangePercentage);

            await this.RunQuery(SqlQuery.InsertRawData, parameters);
        }

        public async Task<StockInfo> GetLastStock(string symbol)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@symbol", symbol);

            DataSet dataSet = await this.GetData(SqlQuery.GetLastDatapoint, parameters);
            StockInfo stockInfo = new StockInfo();
            if (dataSet.Tables != null && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables[0].Rows[0];
                stockInfo.Symbol = row["Symbol"].ToString();

                DateTime dateTimeTemp;
                DateTime.TryParse(row["Timestamp"].ToString(), out dateTimeTemp);
                stockInfo.LastUpdate = dateTimeTemp;

                decimal decimalTemp;
                decimal.TryParse(row["Price"].ToString(), out decimalTemp);
                stockInfo.LastTradePrice = decimalTemp;

                double doubleTemp;
                double.TryParse(row["Volume"].ToString(), out doubleTemp);
                stockInfo.CurrentVolume = doubleTemp;

                double.TryParse(row["ChangePercentage"].ToString(), out doubleTemp);
                stockInfo.ChangePercentage = doubleTemp;
            }

            return stockInfo;
        }

        private async Task<DataSet> GetData(string query, Dictionary<string, object> parameters)
        {
            SqlConnection sqlconnection = new SqlConnection(this.connectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
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
                sqlDataAdapter.SelectCommand = myCommand;
                sqlDataAdapter.Fill(dataSet);
            }
            catch
            {
            }

            return await Task.FromResult<DataSet>(dataSet);
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

