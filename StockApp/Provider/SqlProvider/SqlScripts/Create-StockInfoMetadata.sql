CREATE TABLE [dbo].[StockInfoMetadata]
(
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Symbol] NVARCHAR(32) NOT NULL ,
    [Timestamp] DATETIMEOFFSET NOT NULL,
    [PercentageRange] REAL NOT NULL, 
    [DataPointCount] INT NOT NULL, 
    [Deleted] BIT NOT NULL,
)

CREATE INDEX Symbol
ON [dbo].[StockInfoMetadata] ([Symbol]);

-- DROP TABLE [dbo].[StockInfoRaw] 

-- SELECT * FROM [dbo].[StockInfoMetadata] ORDER BY Timestamp desc