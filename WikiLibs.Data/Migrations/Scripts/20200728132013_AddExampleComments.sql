CREATE TABLE [ExampleComments] (
    [Id] bigint NOT NULL IDENTITY,
    [ExampleId] bigint NOT NULL,
    [UserId] nvarchar(450) NULL,
    [Data] nvarchar(max) NULL,
    [CreationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ExampleComments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ExampleComments_Examples_ExampleId] FOREIGN KEY ([ExampleId]) REFERENCES [Examples] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ExampleComments_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
);

GO

CREATE INDEX [IX_ExampleComments_ExampleId] ON [ExampleComments] ([ExampleId]);

GO

CREATE INDEX [IX_ExampleComments_UserId] ON [ExampleComments] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200728132013_AddExampleComments', N'3.1.2');

GO

