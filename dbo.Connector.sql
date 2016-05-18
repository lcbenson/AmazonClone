CREATE TABLE [dbo].[Connector] (
    [CartIdentifier] INT NOT NULL,
    [ItemName] NVARCHAR(50) NULL,
    [ItemNumber]     INT NULL,
    PRIMARY KEY CLUSTERED ([CartIdentifier] ASC)
);

