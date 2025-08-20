CREATE TABLE [dbo].[books] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [title] VARCHAR(MAX) NULL,
    [author] VARCHAR(MAX) NULL,
    [price] DECIMAL(18,2) NOT NULL,
    [launch_date] DATETIME2(6) NOT NULL
);