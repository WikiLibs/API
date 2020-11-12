CREATE TABLE [Errors] (
    [Id] bigint NOT NULL IDENTITY,
    [Description] nvarchar(max) NULL,
    [ErrorMessage] nvarchar(max) NULL,
    [ErrorData] nvarchar(max) NULL,
    [ErrorDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Errors] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201030152441_AddErrorReporting', N'3.1.2');

GO

