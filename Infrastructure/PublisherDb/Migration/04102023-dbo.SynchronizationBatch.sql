CREATE TABLE [dbo].[SynchronizationBatch] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [SyncId]        UNIQUEIDENTIFIER NOT NULL,
    [BatchNo]          INT              NOT NULL,
    [Offset]           INT              NOT NULL,
    [BatchTotal]       INT       NOT NULL,
    [SyncTotal] INT       NOT NULL,
    [JobId] NVARCHAR(64) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

