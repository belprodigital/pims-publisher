CREATE TABLE [dbo].[SynchronizationItem] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [SessionId]   UNIQUEIDENTIFIER NOT NULL,
    [BatchNo]     INT              NOT NULL,
    [Identifiers] NTEXT            NOT NULL,
    [Propterties] NTEXT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

