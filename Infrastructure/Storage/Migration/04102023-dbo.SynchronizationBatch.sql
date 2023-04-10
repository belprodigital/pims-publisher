CREATE TABLE [dbo].[SynchronizationBatch] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [SessionId]        UNIQUEIDENTIFIER NULL,
    [BatchNo]          INT              NULL,
    [Offset]           INT              NULL,
    [BatchTotal]       NCHAR (10)       NULL,
    [SessionTotalData] NCHAR (10)       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

