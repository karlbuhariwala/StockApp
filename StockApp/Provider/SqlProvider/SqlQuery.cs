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
    , DataPointCount
    , Timestamp
FROM
    [dbo].[StockInfoMetadata]
WHERE
    Deleted = 0";

        public const string GetRangesFromRawData = @"SELECT 
	CAST(Timestamp as date) AS Timestamp
	, Min(ChangePercentage) AS Min
	, Max(ChangePercentage) AS Max
	, Max(ChangePercentage) - Min(ChangePercentage) AS Range
FROM 
	StockInfoRaw
WHERE
    Symbol = @symbol
    AND Timestamp > @dateTime
	AND Deleted = 0
GROUP BY
	CAST(Timestamp as date)
HAVING 
	Min(ChangePercentage) != Max(ChangePercentage)
ORDER BY
	CAST(Timestamp as date)";

        public const string UpdateInsertRangeData = @"UPDATE StockInfoMetadata
SET Symbol = @symbol
, Timestamp = @timestamp
, PercentageRange = @range
, DataPointCount = @count
, Deleted = 0
WHERE
	Symbol = @symbol
IF @@ROWCOUNT=0
    INSERT INTO StockInfoMetadata (Symbol, Timestamp, PercentageRange, DataPointCount, Deleted)
	VALUES (@symbol, @timestamp, @range, @count, 0)";
    }
}
