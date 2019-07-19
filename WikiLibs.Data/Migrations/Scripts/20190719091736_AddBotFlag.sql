ALTER TABLE [Users] ADD [IsBot] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190719091736_AddBotFlag', N'2.2.6-servicing-10079');

GO

