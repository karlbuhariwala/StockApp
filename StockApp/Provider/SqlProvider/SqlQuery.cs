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
    Symbol = @symbol
    AND Deleted = 0
ORDER BY
    Timestamp desc";

        public const string GetLastStockScore = @"SELECT TOP 1
    Symbol
    , Timestamp
    , Score
FROM
    [dbo].[StockInfoScore]
WHERE
    Symbol = @symbol
    AND Deleted = 0
ORDER BY
    Timestamp desc";

        public const string GetStockInfoToProcess = @"SELECT
    Symbol
    , Timestamp
    , Price
    , Volume
    , ChangePercentage
FROM
    [dbo].[StockInfoRaw]
WHERE
    Symbol = @symbol
    AND Timestamp > @dateTime
    AND Deleted = 0
ORDER BY
    Timestamp";

        public const string GetStockRanges = @"SELECT
    Symbol
    , PercentageRange
FROM
    [dbo].[StockInfoMetadata]
WHERE
    Deleted = 0";
    }
}
