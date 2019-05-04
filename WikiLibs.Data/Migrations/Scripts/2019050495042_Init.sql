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

