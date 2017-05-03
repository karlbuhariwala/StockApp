CREATE TABLE [dbo].[StockInfoRaw]
(
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Symbol] NVARCHAR(32) NOT NULL ,
    [Timestamp] DATETIMEOFFSET NOT NULL,
	[Price] REAL NOT NULL, 
    [Volume] INT NOT NULL, 
    [Deleted] BIT NOT NULL,
)

CREATE INDEX Symbol
ON [dbo].[StockInfoRaw] ([Symbol]);