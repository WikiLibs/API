IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [APIKeys] (
    [Id] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Flags] int NOT NULL,
    [UseNum] int NOT NULL,
    [ExpirationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_APIKeys] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Groups] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [InfoTable] (
    [Id] bigint NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [Data] nvarchar(max) NULL,
    CONSTRAINT [PK_InfoTable] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Permissions] (
    [Id] bigint NOT NULL IDENTITY,
    [GroupId] bigint NOT NULL,
    [Perm] nvarchar(max) NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Permissions_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Users] (
    [Id] nvarchar(450) NOT NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Icon] nvarchar(max) NULL,
    [EMail] nvarchar(max) NULL,
    [Confirmation] nvarchar(max) NULL,
    [Private] bit NOT NULL,
    [ProfileMsg] nvarchar(max) NULL,
    [Points] int NOT NULL,
    [Pseudo] nvarchar(max) NULL,
    [GroupId] bigint NULL,
    [Pass] nvarchar(max) NULL,
    [RegistrationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Symbols] (
    [Id] bigint NOT NULL IDENTITY,
    [Path] nvarchar(max) NULL,
    [UserId] nvarchar(450) NULL,
    [Lang] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    [CreationDate] datetime2 NOT NULL,
    [LastModificationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Symbols] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Symbols_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Examples] (
    [Id] bigint NOT NULL IDENTITY,
    [SymbolId] bigint NOT NULL,
    [Code] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [UserId] nvarchar(450) NULL,
    [CreationDate] datetime2 NOT NULL,
    [LastModificationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Examples] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Examples_Symbols_SymbolId] FOREIGN KEY ([SymbolId]) REFERENCES [Symbols] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Examples_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Prototypes] (
    [Id] bigint NOT NULL IDENTITY,
    [SymbolId] bigint NOT NULL,
    [Data] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_Prototypes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Prototypes_Symbols_SymbolId] FOREIGN KEY ([SymbolId]) REFERENCES [Symbols] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [SymbolRefs] (
    [Id] bigint NOT NULL IDENTITY,
    [SymbolId] bigint NOT NULL,
    [Path] nvarchar(max) NULL,
    CONSTRAINT [PK_SymbolRefs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SymbolRefs_Symbols_SymbolId] FOREIGN KEY ([SymbolId]) REFERENCES [Symbols] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [PrototypeParams] (
    [Id] bigint NOT NULL IDENTITY,
    [PrototypeId] bigint NOT NULL,
    [Data] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Path] nvarchar(max) NULL,
    CONSTRAINT [PK_PrototypeParams] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PrototypeParams_Prototypes_PrototypeId] FOREIGN KEY ([PrototypeId]) REFERENCES [Prototypes] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Examples_SymbolId] ON [Examples] ([SymbolId]);

GO

CREATE INDEX [IX_Examples_UserId] ON [Examples] ([UserId]);

GO

CREATE INDEX [IX_Permissions_GroupId] ON [Permissions] ([GroupId]);

GO

CREATE INDEX [IX_PrototypeParams_PrototypeId] ON [PrototypeParams] ([PrototypeId]);

GO

CREATE INDEX [IX_Prototypes_SymbolId] ON [Prototypes] ([SymbolId]);

GO

CREATE INDEX [IX_SymbolRefs_SymbolId] ON [SymbolRefs] ([SymbolId]);

GO

CREATE INDEX [IX_Symbols_UserId] ON [Symbols] ([UserId]);

GO

CREATE INDEX [IX_Users_GroupId] ON [Users] ([GroupId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190504095042_Init', N'2.1.8-servicing-32085');

GO

ALTER TABLE [Examples] DROP CONSTRAINT [FK_Examples_Users_UserId];

GO

ALTER TABLE [Symbols] DROP CONSTRAINT [FK_Symbols_Users_UserId];

GO

ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Groups_GroupId];

GO

DROP INDEX [IX_Users_GroupId] ON [Users];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Examples]') AND [c].[name] = N'Code');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Examples] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Examples] DROP COLUMN [Code];

GO

ALTER TABLE [Examples] ADD [RequestId] bigint NULL;

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
    CONSTRAINT [FK_ExampleRequests_Examples_DataId] FOREIGN KEY ([DataId]) REFERENCES [Examples] ([Id]) ON DELETE SET NULL
);

GO

CREATE UNIQUE INDEX [IX_Users_GroupId] ON [Users] ([GroupId]) WHERE [GroupId] IS NOT NULL;

GO

CREATE INDEX [IX_ExampleCodeLines_ExampleId] ON [ExampleCodeLines] ([ExampleId]);

GO

CREATE INDEX [IX_ExampleRequests_ApplyToId] ON [ExampleRequests] ([ApplyToId]);

GO

CREATE UNIQUE INDEX [IX_ExampleRequests_DataId] ON [ExampleRequests] ([DataId]) WHERE [DataId] IS NOT NULL;

GO

ALTER TABLE [Examples] ADD CONSTRAINT [FK_Examples_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL;

GO

ALTER TABLE [Symbols] ADD CONSTRAINT [FK_Symbols_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL;

GO

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE SET NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190601204231_AddExamples', N'2.1.8-servicing-32085');

GO

