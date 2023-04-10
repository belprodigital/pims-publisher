CREATE TABLE [dbo].[SynchronizationSession] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [ProjectCode]       NVARCHAR (16)    NOT NULL,
    [ModelCode]         NVARCHAR (16)    NOT NULL,
    [TotalItem]         INT              NOT NULL,
    [TotalBatch]        INT              NOT NULL,
    [CreatedAt]         DATETIME         NOT NULL,
    [UpdatedAt]         DATETIME         NOT NULL,
    [TotalSumitted]     INT              NOT NULL,
    [Progress]          INT              NULL,
    [Status]            NVARCHAR (64)    NULL,
    [CreatedBy]         NVARCHAR (128)   NULL,
    [UpdatedBy]         NVARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

