EXEC sp_rename N'[Users].[EMail]', N'Email', N'COLUMN';

GO

ALTER TABLE [Users] ADD [IsBot] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190724124228_AddBotFlag', N'2.2.6-servicing-10079');

GO

