IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [ApiKeys] (
    [Id] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Flags] int NOT NULL,
    [UseNum] int NOT NULL,
    [ExpirationDate] datetime2 NOT NULL,
    [Origin] nvarchar(max) NULL,
    CONSTRAINT [PK_ApiKeys] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Groups] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [SymbolImports] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_SymbolImports] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [SymbolLangs] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Icon] varbinary(max) NULL,
    CONSTRAINT [PK_SymbolLangs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [SymbolLibs] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_SymbolLibs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [SymbolTypes] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_SymbolTypes] PRIMARY KEY ([Id])
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
    [Icon] varbinary(max) NULL,
    [Email] nvarchar(max) NULL,
    [Confirmation] nvarchar(max) NULL,
    [Private] bit NOT NULL,
    [ProfileMsg] nvarchar(max) NULL,
    [Points] int NOT NULL,
    [Pseudo] nvarchar(max) NULL,
    [GroupId] bigint NULL,
    [Pass] nvarchar(max) NULL,
    [RegistrationDate] datetime2 NOT NULL,
    [IsBot] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE SET NULL
);

GO

CREATE TABLE [Symbols] (
    [Id] bigint NOT NULL IDENTITY,
    [Path] nvarchar(450) NULL,
    [UserId] nvarchar(450) NULL,
    [LangId] bigint NOT NULL,
    [LibId] bigint NOT NULL,
    [TypeId] bigint NOT NULL,
    [ImportId] bigint NULL,
    [Views] bigint NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    [LastModificationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Symbols] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Symbols_SymbolImports_ImportId] FOREIGN KEY ([ImportId]) REFERENCES [SymbolImports] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Symbols_SymbolLangs_LangId] FOREIGN KEY ([LangId]) REFERENCES [SymbolLangs] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Symbols_SymbolLibs_LibId] FOREIGN KEY ([LibId]) REFERENCES [SymbolLibs] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Symbols_SymbolTypes_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [SymbolTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Symbols_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
);

GO

CREATE TABLE [Examples] (
    [Id] bigint NOT NULL IDENTITY,
    [SymbolId] bigint NOT NULL,
    [RequestId] bigint NULL,
    [UserId] nvarchar(450) NULL,
    [Description] nvarchar(max) NULL,
    [CreationDate] datetime2 NOT NULL,
    [LastModificationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Examples] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Examples_Symbols_SymbolId] FOREIGN KEY ([SymbolId]) REFERENCES [Symbols] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Examples_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
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
    [RefId] bigint NULL,
    [RefPath] nvarchar(max) NULL,
    CONSTRAINT [PK_SymbolRefs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SymbolRefs_Symbols_RefId] FOREIGN KEY ([RefId]) REFERENCES [Symbols] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SymbolRefs_Symbols_SymbolId] FOREIGN KEY ([SymbolId]) REFERENCES [Symbols] ([Id]) ON DELETE CASCADE
);

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

CREATE TABLE [PrototypeParams] (
    [Id] bigint NOT NULL IDENTITY,
    [PrototypeId] bigint NOT NULL,
    [Data] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_PrototypeParams] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PrototypeParams_Prototypes_PrototypeId] FOREIGN KEY ([PrototypeId]) REFERENCES [Prototypes] ([Id]) ON DELETE CASCADE
);

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

CREATE INDEX [IX_ExampleCodeLines_ExampleId] ON [ExampleCodeLines] ([ExampleId]);

GO

CREATE INDEX [IX_ExampleRequests_ApplyToId] ON [ExampleRequests] ([ApplyToId]);

GO

CREATE UNIQUE INDEX [IX_ExampleRequests_DataId] ON [ExampleRequests] ([DataId]) WHERE [DataId] IS NOT NULL;

GO

CREATE INDEX [IX_Examples_SymbolId] ON [Examples] ([SymbolId]);

GO

CREATE INDEX [IX_Examples_UserId] ON [Examples] ([UserId]);

GO

CREATE INDEX [IX_Permissions_GroupId] ON [Permissions] ([GroupId]);

GO

CREATE INDEX [IX_PrototypeParams_PrototypeId] ON [PrototypeParams] ([PrototypeId]);

GO

CREATE UNIQUE INDEX [IX_PrototypeParamSymbolRefs_PrototypeParamId] ON [PrototypeParamSymbolRefs] ([PrototypeParamId]);

GO

CREATE INDEX [IX_PrototypeParamSymbolRefs_RefId] ON [PrototypeParamSymbolRefs] ([RefId]);

GO

CREATE INDEX [IX_Prototypes_SymbolId] ON [Prototypes] ([SymbolId]);

GO

CREATE INDEX [IX_SymbolRefs_RefId] ON [SymbolRefs] ([RefId]);

GO

CREATE INDEX [IX_SymbolRefs_SymbolId] ON [SymbolRefs] ([SymbolId]);

GO

CREATE INDEX [IX_Symbols_ImportId] ON [Symbols] ([ImportId]);

GO

CREATE INDEX [IX_Symbols_LangId] ON [Symbols] ([LangId]);

GO

CREATE INDEX [IX_Symbols_LibId] ON [Symbols] ([LibId]);

GO

CREATE UNIQUE INDEX [IX_Symbols_Path] ON [Symbols] ([Path]) WHERE [Path] IS NOT NULL;

GO

CREATE INDEX [IX_Symbols_TypeId] ON [Symbols] ([TypeId]);

GO

CREATE INDEX [IX_Symbols_UserId] ON [Symbols] ([UserId]);

GO

CREATE INDEX [IX_Users_GroupId] ON [Users] ([GroupId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190804151124_Init', N'2.2.6-servicing-10079');

GO

