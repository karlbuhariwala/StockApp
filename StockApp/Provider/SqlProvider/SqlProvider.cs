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

                DateTimeOffset dateTimeOffsetTemp;
                DateTimeOffset.TryParse(row["Timestamp"].ToString(), out dateTimeOffsetTemp);
                stockInfo.LastUpdate = dateTimeOffsetTemp;

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

        public async Task<StockScore> GetLastStockScore(string symbol)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@symbol", symbol);

            DataSet dataSet = await this.GetData(SqlQuery.GetLastStockScore, parameters);
            StockScore stockScore = new StockScore();
            if (dataSet.Tables != null && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables[0].Rows[0];
                stockScore.Symbol = row["Symbol"].ToString();

                DateTimeOffset dateTimeOffsetTemp;
                DateTimeOffset.TryParse(row["Timestamp"].ToString(), out dateTimeOffsetTemp);
                stockScore.LastUpdate = dateTimeOffsetTemp;

                int intTemp;
                int.TryParse(row["Score"].ToString(), out intTemp);
                stockScore.Score = intTemp;
            }

            return stockScore;
        }

        public async Task<List<StockInfo>> GetStockPricesToProcess(string symbol, DateTimeOffset dateTime)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@symbol", symbol);
            parameters.Add("@dateTime", dateTime);

            DataSet dataSet = await this.GetData(SqlQuery.GetStockInfoToProcess, parameters);
            List<StockInfo> stockInfoList = new List<StockInfo>();
            if (dataSet.Tables != null && dataSet.Tables[0].Rows != null)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    StockInfo stockInfo = new StockInfo();
                    stockInfo.Symbol = row["Symbol"].ToString();

                    DateTimeOffset dateTimeOffsetTemp;
                    DateTimeOffset.TryParse(row["Timestamp"].ToString(), out dateTimeOffsetTemp);
                    stockInfo.LastUpdate = dateTimeOffsetTemp;

                    decimal decimalTemp;
                    decimal.TryParse(row["Price"].ToString(), out decimalTemp);
                    stockInfo.LastTradePrice = decimalTemp;

                    double doubleTemp;
                    double.TryParse(row["Volume"].ToString(), out doubleTemp);
                    stockInfo.CurrentVolume = doubleTemp;

                    double.TryParse(row["ChangePercentage"].ToString(), out doubleTemp);
                    stockInfo.ChangePercentage = doubleTemp;

                    stockInfoList.Add(stockInfo);
                }
            }

            return stockInfoList;
        }

        public async Task<Dictionary<string, StockMetadataInfo>> GetStockRanges()
        {
            DataSet dataSet = await this.GetData(SqlQuery.GetStockRanges);
            Dictionary<string, StockMetadataInfo> stockIdentityRangeMap = new Dictionary<string, StockMetadataInfo>(StringComparer.OrdinalIgnoreCase);
            if (dataSet.Tables != null && dataSet.Tables[0].Rows != null)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    string symbol = row["Symbol"].ToString();

                    double percentageRange;
                    double.TryParse(row["PercentageRange"].ToString(), out percentageRange);

                    int dataPointCount;
                    int.TryParse(row["DataPointCount"].ToString(), out dataPointCount);

                    DateTimeOffset lastDataPointDateTime;
                    DateTimeOffset.TryParse(row["Timestamp"].ToString(), out lastDataPointDateTime);

                    stockIdentityRangeMap.Add(symbol, new StockMetadataInfo() { Symbol = symbol, PercentageChangeRange = percentageRange, DataPointCount = dataPointCount, LastProcessedDateTime = lastDataPointDateTime });
                }
            }

            return stockIdentityRangeMap;
        }

        public async Task BulkInsertScores(List<StockScore> stockScores)
        {
            DataTable table = new DataTable("StockInfoScore");
            table.Columns.Add("Symbol");
            table.Columns.Add("Timestamp", typeof(DateTime));
            table.Columns.Add("Score", typeof(int));
            table.Columns.Add("Deleted", typeof(bool));

            foreach (var item in stockScores)
            {
                DataRow row = table.NewRow();
                row["Symbol"] = item.Symbol;
                row["Timestamp"] = item.LastUpdate;
                row["Score"] = item.Score;
                row["Deleted"] = false;

                table.Rows.Add(row);
            }

            await this.BulkInsert(table);
        }

        public async Task<Tuple<List<double>, DateTime>> GetRangesSinceLastProcessedDay(string symbol, DateTime dateTime)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@symbol", symbol);
            parameters.Add("@dateTime", dateTime);

            List<double> ranges = new List<double>();
            DateTime lastProcessedDateTime;
            DataSet dataSet = await this.GetData(SqlQuery.GetRangesFromRawData, parameters);
            if (dataSet.Tables != null && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    double doubleTemp;
                    double.TryParse(row["Range"].ToString(), out doubleTemp);

                    ranges.Add(doubleTemp);
                }

                DateTime dateTimeTemp;
                string lastDateTime = dataSet.Tables[0].Rows[dataSet.Tables[0].Rows.Count - 1]["Timestamp"].ToString();
                DateTime.TryParse(lastDateTime, out dateTimeTemp);
                lastProcessedDateTime = dateTimeTemp;
            }
            else
            {
                lastProcessedDateTime = dateTime;
            }

            return new Tuple<List<double>, DateTime>(ranges, lastProcessedDateTime);
        }

        public async Task SaveRange(string symbol, double newRange, int count, DateTime lastProcessedDateTime)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@symbol", symbol);
            parameters.Add("@timestamp", lastProcessedDateTime);
            parameters.Add("@range", newRange);
            parameters.Add("@count", count);

            await this.RunQuery(SqlQuery.UpdateInsertRangeData, parameters);
        }

        private async Task BulkInsert(DataTable table)
        {
            SqlConnection sqlconnection = new SqlConnection(this.connectionString);
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlconnection);
            foreach (DataColumn column in table.Columns)
            {
                sqlBulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            }

            sqlBulkCopy.DestinationTableName = table.TableName;
            try
            {
                sqlconnection.Open();
                await sqlBulkCopy.WriteToServerAsync(table);
                sqlconnection.Close();
            }
            catch
            {
            }
        }

        private async Task<DataSet> GetData(string query, Dictionary<string, object> parameters = null)
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

        private async Task RunQuery(string query, Dictionary<string, object> parameters = null)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

