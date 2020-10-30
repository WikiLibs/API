CREATE TABLE [Errors] (
    [Id] bigint NOT NULL IDENTITY,
    [Description] nvarchar(max) NULL,
    [ErrorMessage] nvarchar(max) NULL,
    [ErrorData] nvarchar(max) NULL,
    CONSTRAINT [PK_Errors] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201030143506_AddErrorReporting', N'3.1.2');

GO

