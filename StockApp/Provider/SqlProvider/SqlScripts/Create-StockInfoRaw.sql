CREATE TABLE [dbo].[StockInfoRaw]
(
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Symbol] NVARCHAR(32) NOT NULL ,
    [Timestamp] DATETIMEOFFSET NOT NULL,
	[Price] REAL NOT NULL, 
    [Volume] INT NOT NULL, 
    [ChangePercentage] REAL NOT NULL, 
    [Deleted] BIT NOT NULL,
)

CREATE INDEX Symbol
ON [dbo].[StockInfoRaw] ([Symbol]);

-- DROP TABLE [dbo].[StockInfoRaw] 

-- SELECT * FROM [dbo].[StockInfoRaw] WHERE symbol  = 'MSFT' ORDER BY Timestamp desc

--UPDATE [dbo].[StockInfoRaw]
SET Deleted = 1
WHERE Id = 5457

SELECT 
	symbol
	, MIN(ChangePercentage)
	, MAX(ChangePercentage)
FROM
	[dbo].[StockInfoRaw]
WHERE
	cast([Timestamp] as date) = '2017-05-04'
	AND symbol = 'MSFT'
	AND ChangePercentage != 0
GROUP BY
	symbol


SELECT 
	*
FROM
	[dbo].[StockInfoRaw]
WHERE
	cast([Timestamp] as date) = '2017-05-04'
	AND symbol = 'MSFT'
ORDER BY
	[Timestamp]