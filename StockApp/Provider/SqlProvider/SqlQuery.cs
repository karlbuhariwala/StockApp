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
    , ChangePercentage
    , Deleted
)
VALUES
    (
        @symbol
        , @timestamp
        , @price
        , @volume
        , @changePercentage
        , 0
    )";

        public const string GetLastDatapoint = @"SELECT TOP 1
    Symbol
    , Timestamp
    , Price
    , Volume
    , ChangePercentage
FROM
    [dbo].[StockInfoRaw]
WHERE
    Symbol = 'MSFT'
    AND Deleted = 0
ORDER BY
    Timestamp desc";
    }
}
