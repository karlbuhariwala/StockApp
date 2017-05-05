CREATE TABLE [dbo].[StockInfoScore]
(
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Symbol] NVARCHAR(32) NOT NULL ,
    [Timestamp] DATETIMEOFFSET NOT NULL,
    [Score] INT NOT NULL, 
    [Deleted] BIT NOT NULL,
)

CREATE INDEX Symbol
ON [dbo].[StockInfoScore] ([Symbol]);

-- DROP TABLE [dbo].[StockInfoRaw] 

-- SELECT * FROM [dbo].[StockInfoScore] ORDER BY Timestamp desc

