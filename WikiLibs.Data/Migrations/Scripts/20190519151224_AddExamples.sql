ALTER TABLE [Examples] DROP CONSTRAINT [FK_Examples_Symbols_SymbolId];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Examples]') AND [c].[name] = N'Code');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Examples] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Examples] DROP COLUMN [Code];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Examples]') AND [c].[name] = N'SymbolId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Examples] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Examples] ALTER COLUMN [SymbolId] bigint NULL;

GO

ALTER TABLE [Examples] ADD [State] int NOT NULL DEFAULT 0;

GO

CREATE TABLE [ExampleCodeLines] (
    [Id] bigint NOT NULL IDENTITY,
    [ExampleId] bigint NULL,
    [Data] nvarchar(max) NULL,
    [Comment] nvarchar(max) NULL,
    CONSTRAINT [PK_ExampleCodeLines] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ExampleCodeLines_Examples_ExampleId] FOREIGN KEY ([ExampleId]) REFERENCES [Examples] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_ExampleCodeLines_ExampleId] ON [ExampleCodeLines] ([ExampleId]);

GO

ALTER TABLE [Examples] ADD CONSTRAINT [FK_Examples_Symbols_SymbolId] FOREIGN KEY ([SymbolId]) REFERENCES [Symbols] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190519151224_AddExamples', N'2.1.8-servicing-32085');

GO

