// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = SqlQuery.cs

namespace StockApp.Provider.SqlProvider
{
    public static class SqlQuery
    {
        public const string InsertRawData = @"INSERT INTO [dbo].[StockInfoRaw]
(
    Symbol
    , Timestamp
    , Price
    , Volume
    , Deleted
)
VALUES
    (
        @symbol
        , @timestamp
        , @price
        , @volume
        , 0
    )";
    }
}
