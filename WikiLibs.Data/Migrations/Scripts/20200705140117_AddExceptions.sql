CREATE TABLE [Exceptions] (
    [Id] bigint NOT NULL IDENTITY,
    [SymbolId] bigint NOT NULL,
    [RefId] bigint NULL,
    [RefPath] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_Exceptions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Exceptions_Symbols_SymbolId] FOREIGN KEY ([SymbolId]) REFERENCES [Symbols] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Exceptions_SymbolId] ON [Exceptions] ([SymbolId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200705140117_AddExceptions', N'3.1.2');

GO

