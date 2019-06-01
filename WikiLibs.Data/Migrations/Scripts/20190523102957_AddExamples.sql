ALTER TABLE [Examples] DROP CONSTRAINT [FK_Examples_Users_UserId];

GO

ALTER TABLE [Symbols] DROP CONSTRAINT [FK_Symbols_Users_UserId];

GO

ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Groups_GroupId];

GO

DROP INDEX [IX_Users_GroupId] ON [Users];

GO

DROP INDEX [IX_Symbols_UserId] ON [Symbols];

GO

DROP INDEX [IX_Examples_UserId] ON [Examples];

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

CREATE UNIQUE INDEX [IX_Symbols_UserId] ON [Symbols] ([UserId]) WHERE [UserId] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_Examples_UserId] ON [Examples] ([UserId]) WHERE [UserId] IS NOT NULL;

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
VALUES (N'20190523102957_AddExamples', N'2.1.11-servicing-32099');

GO

