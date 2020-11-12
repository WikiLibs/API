ALTER TABLE [SymbolLibs] ADD [Copyright] nvarchar(max) NULL;

GO

ALTER TABLE [SymbolLibs] ADD [Description] nvarchar(max) NULL;

GO

ALTER TABLE [SymbolLibs] ADD [DisplayName] nvarchar(max) NULL;

GO

ALTER TABLE [SymbolLibs] ADD [Icon] varbinary(max) NULL;

GO

ALTER TABLE [SymbolLibs] ADD [UserId] nvarchar(450) NULL;

GO

CREATE INDEX [IX_SymbolLibs_UserId] ON [SymbolLibs] ([UserId]);

GO

ALTER TABLE [SymbolLibs] ADD CONSTRAINT [FK_SymbolLibs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201107142540_AddLibPage', N'3.1.2');

GO

