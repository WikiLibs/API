DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Examples]') AND [c].[name] = N'Code');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Examples] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Examples] DROP COLUMN [Code];

GO

CREATE TABLE [ExampleCodeLines] (
    [Id] bigint NOT NULL IDENTITY,
    [ExampleId] bigint NOT NULL,
    [Data] nvarchar(max) NULL,
    [Comment] nvarchar(max) NULL,
    CONSTRAINT [PK_ExampleCodeLines] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ExampleCodeLines_Examples_ExampleId] FOREIGN KEY ([ExampleId]) REFERENCES [Examples] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [ExampleRequests] (
    [Id] bigint NOT NULL IDENTITY,
    [DataId] bigint NULL,
    [ApplyToId] bigint NULL,
    [Message] nvarchar(max) NULL,
    [CreationDate] datetime2 NOT NULL,
    [Type] int NOT NULL,
    CONSTRAINT [PK_ExampleRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ExampleRequests_Examples_ApplyToId] FOREIGN KEY ([ApplyToId]) REFERENCES [Examples] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ExampleRequests_Examples_DataId] FOREIGN KEY ([DataId]) REFERENCES [Examples] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_ExampleCodeLines_ExampleId] ON [ExampleCodeLines] ([ExampleId]);

GO

CREATE INDEX [IX_ExampleRequests_ApplyToId] ON [ExampleRequests] ([ApplyToId]);

GO

CREATE INDEX [IX_ExampleRequests_DataId] ON [ExampleRequests] ([DataId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190519162116_AddExamples', N'2.1.8-servicing-32085');

GO

