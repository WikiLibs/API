DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrototypeParams]') AND [c].[name] = N'Path');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PrototypeParams] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [PrototypeParams] DROP COLUMN [Path];

GO

EXEC sp_rename N'[SymbolRefs].[Path]', N'RefPath', N'COLUMN';

GO

ALTER TABLE [Symbols] ADD [Lib] nvarchar(max) NULL;

GO

ALTER TABLE [SymbolRefs] ADD [RefId] bigint NULL;

GO

ALTER TABLE [PrototypeParams] ADD [SymbolRefId] bigint NULL;

GO

CREATE TABLE [PrototypeParamSymbolRefs] (
    [Id] bigint NOT NULL IDENTITY,
    [PrototypeParamId] bigint NOT NULL,
    [RefId] bigint NULL,
    [RefPath] nvarchar(max) NULL,
    CONSTRAINT [PK_PrototypeParamSymbolRefs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PrototypeParamSymbolRefs_PrototypeParams_PrototypeParamId] FOREIGN KEY ([PrototypeParamId]) REFERENCES [PrototypeParams] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PrototypeParamSymbolRefs_Symbols_RefId] FOREIGN KEY ([RefId]) REFERENCES [Symbols] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_SymbolRefs_RefId] ON [SymbolRefs] ([RefId]);

GO

CREATE UNIQUE INDEX [IX_PrototypeParamSymbolRefs_PrototypeParamId] ON [PrototypeParamSymbolRefs] ([PrototypeParamId]);

GO

CREATE INDEX [IX_PrototypeParamSymbolRefs_RefId] ON [PrototypeParamSymbolRefs] ([RefId]);

GO

ALTER TABLE [SymbolRefs] ADD CONSTRAINT [FK_SymbolRefs_Symbols_RefId] FOREIGN KEY ([RefId]) REFERENCES [Symbols] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190602120553_OptimizeSymbols', N'2.1.8-servicing-32085');

GO

