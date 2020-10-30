ALTER TABLE [Examples] ADD [VoteCount] int NOT NULL DEFAULT 0;

GO

CREATE TABLE [ExampleVotes] (
    [UserId] nvarchar(450) NOT NULL,
    [ExampleId] bigint NOT NULL,
    CONSTRAINT [PK_ExampleVotes] PRIMARY KEY ([UserId], [ExampleId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201030092928_AddExampleVotes', N'3.1.2');

GO

