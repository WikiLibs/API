CREATE TABLE [Exceptions] (
    [Id] bigint NOT NULL IDENTITY,
    [PrototypeId] bigint NOT NULL,
    [RefId] bigint NULL,
    [RefPath] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_Exceptions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Exceptions_Prototypes_PrototypeId] FOREIGN KEY ([PrototypeId]) REFERENCES [Prototypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Exceptions_Symbols_RefId] FOREIGN KEY ([RefId]) REFERENCES [Symbols] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Exceptions_PrototypeId] ON [Exceptions] ([PrototypeId]);

GO

CREATE INDEX [IX_Exceptions_RefId] ON [Exceptions] ([RefId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200711092938_AddExceptions', N'3.1.2');

GO

