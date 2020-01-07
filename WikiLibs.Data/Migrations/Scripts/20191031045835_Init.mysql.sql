CREATE TABLE IF NOT EXISTS __EFMigrationsHistory (
    MigrationId nvarchar(150) NOT NULL,
    ProductVersion nvarchar(32) NOT NULL,
    CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);

CREATE TABLE ApiKeys (
    Id nvarchar(450) NOT NULL,
    Description LONGTEXT NULL,
    Flags int NOT NULL,
    UseNum int NOT NULL,
    ExpirationDate DATETIME(6) NOT NULL,
    Origin LONGTEXT NULL,
    CONSTRAINT PK_ApiKeys PRIMARY KEY (Id)
);

CREATE TABLE Groups (
    Id bigint NOT NULL AUTO_INCREMENT,
    Name LONGTEXT NULL,
    CONSTRAINT PK_Groups PRIMARY KEY (Id)
);

CREATE TABLE SymbolImports (
    Id bigint NOT NULL AUTO_INCREMENT,
    Name LONGTEXT NULL,
    CONSTRAINT PK_SymbolImports PRIMARY KEY (Id)
);

CREATE TABLE SymbolLangs (
    Id bigint NOT NULL AUTO_INCREMENT,
    Name nvarchar(450) NULL,
    DisplayName LONGTEXT NULL,
    Icon LONGBLOB NULL,
    CONSTRAINT PK_SymbolLangs PRIMARY KEY (Id)
);

CREATE TABLE SymbolLibs (
    Id bigint NOT NULL AUTO_INCREMENT,
    Name LONGTEXT NULL,
    CONSTRAINT PK_SymbolLibs PRIMARY KEY (Id)
);

CREATE TABLE SymbolTypes (
    Id bigint NOT NULL AUTO_INCREMENT,
    Name nvarchar(450) NULL,
    DisplayName LONGTEXT NULL,
    CONSTRAINT PK_SymbolTypes PRIMARY KEY (Id)
);

CREATE TABLE Permissions (
    Id bigint NOT NULL AUTO_INCREMENT,
    GroupId bigint NOT NULL,
    Perm LONGTEXT NULL,
    CONSTRAINT PK_Permissions PRIMARY KEY (Id),
    CONSTRAINT FK_Permissions_Groups_GroupId FOREIGN KEY (GroupId) REFERENCES Groups (Id) ON DELETE CASCADE
);

CREATE TABLE Users (
    Id nvarchar(450) NOT NULL,
    FirstName LONGTEXT NULL,
    LastName LONGTEXT NULL,
    Icon LONGBLOB NULL,
    Email LONGTEXT NULL,
    Confirmation LONGTEXT NULL,
    Private bit NOT NULL,
    ProfileMsg LONGTEXT NULL,
    Points int NOT NULL,
    Pseudo LONGTEXT NULL,
    GroupId bigint NULL,
    Pass LONGTEXT NULL,
    RegistrationDate DATETIME(6) NOT NULL,
    IsBot bit NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY (Id),
    CONSTRAINT FK_Users_Groups_GroupId FOREIGN KEY (GroupId) REFERENCES Groups (Id) ON DELETE SET NULL
);

CREATE TABLE Symbols (
    Id bigint NOT NULL AUTO_INCREMENT,
    Path nvarchar(450) NULL,
    UserId nvarchar(450) NULL,
    LangId bigint NOT NULL,
    LibId bigint NOT NULL,
    TypeId bigint NOT NULL,
    ImportId bigint NULL,
    Views bigint NOT NULL,
    CreationDate DATETIME(6) NOT NULL,
    LastModificationDate DATETIME(6) NOT NULL,
    CONSTRAINT PK_Symbols PRIMARY KEY (Id),
    CONSTRAINT FK_Symbols_SymbolImports_ImportId FOREIGN KEY (ImportId) REFERENCES SymbolImports (Id) ON DELETE SET NULL,
    CONSTRAINT FK_Symbols_SymbolLangs_LangId FOREIGN KEY (LangId) REFERENCES SymbolLangs (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Symbols_SymbolLibs_LibId FOREIGN KEY (LibId) REFERENCES SymbolLibs (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Symbols_SymbolTypes_TypeId FOREIGN KEY (TypeId) REFERENCES SymbolTypes (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Symbols_Users_UserId FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE SET NULL
);

CREATE TABLE Examples (
    Id bigint NOT NULL AUTO_INCREMENT,
    SymbolId bigint NOT NULL,
    RequestId bigint NULL,
    UserId nvarchar(450) NULL,
    Description LONGTEXT NULL,
    CreationDate DATETIME(6) NOT NULL,
    LastModificationDate DATETIME(6) NOT NULL,
    CONSTRAINT PK_Examples PRIMARY KEY (Id),
    CONSTRAINT FK_Examples_Symbols_SymbolId FOREIGN KEY (SymbolId) REFERENCES Symbols (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Examples_Users_UserId FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE SET NULL
);

CREATE TABLE Prototypes (
    Id bigint NOT NULL AUTO_INCREMENT,
    SymbolId bigint NOT NULL,
    Data LONGTEXT NULL,
    Description LONGTEXT NULL,
    CONSTRAINT PK_Prototypes PRIMARY KEY (Id),
    CONSTRAINT FK_Prototypes_Symbols_SymbolId FOREIGN KEY (SymbolId) REFERENCES Symbols (Id) ON DELETE CASCADE
);

CREATE TABLE SymbolRefs (
    Id bigint NOT NULL AUTO_INCREMENT,
    SymbolId bigint NOT NULL,
    RefId bigint NULL,
    RefPath LONGTEXT NULL,
    CONSTRAINT PK_SymbolRefs PRIMARY KEY (Id),
    CONSTRAINT FK_SymbolRefs_Symbols_RefId FOREIGN KEY (RefId) REFERENCES Symbols (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_SymbolRefs_Symbols_SymbolId FOREIGN KEY (SymbolId) REFERENCES Symbols (Id) ON DELETE CASCADE
);

CREATE TABLE ExampleCodeLines (
    Id bigint NOT NULL AUTO_INCREMENT,
    ExampleId bigint NOT NULL,
    Data LONGTEXT NULL,
    Comment LONGTEXT NULL,
    CONSTRAINT PK_ExampleCodeLines PRIMARY KEY (Id),
    CONSTRAINT FK_ExampleCodeLines_Examples_ExampleId FOREIGN KEY (ExampleId) REFERENCES Examples (Id) ON DELETE CASCADE
);

CREATE TABLE ExampleRequests (
    Id bigint NOT NULL AUTO_INCREMENT,
    DataId bigint NULL,
    ApplyToId bigint NULL,
    Message LONGTEXT NULL,
    CreationDate DATETIME(6) NOT NULL,
    Type int NOT NULL,
    CONSTRAINT PK_ExampleRequests PRIMARY KEY (Id),
    CONSTRAINT FK_ExampleRequests_Examples_ApplyToId FOREIGN KEY (ApplyToId) REFERENCES Examples (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_ExampleRequests_Examples_DataId FOREIGN KEY (DataId) REFERENCES Examples (Id) ON DELETE SET NULL
);

CREATE TABLE PrototypeParams (
    Id bigint NOT NULL AUTO_INCREMENT,
    PrototypeId bigint NOT NULL,
    Data LONGTEXT NULL,
    Description LONGTEXT NULL,
    CONSTRAINT PK_PrototypeParams PRIMARY KEY (Id),
    CONSTRAINT FK_PrototypeParams_Prototypes_PrototypeId FOREIGN KEY (PrototypeId) REFERENCES Prototypes (Id) ON DELETE CASCADE
);

CREATE TABLE PrototypeParamSymbolRefs (
    Id bigint NOT NULL AUTO_INCREMENT,
    PrototypeParamId bigint NOT NULL,
    RefId bigint NULL,
    RefPath LONGTEXT NULL,
    CONSTRAINT PK_PrototypeParamSymbolRefs PRIMARY KEY (Id),
    CONSTRAINT FK_PrototypeParamSymbolRefs_PrototypeParams_PrototypeParamId FOREIGN KEY (PrototypeParamId) REFERENCES PrototypeParams (Id) ON DELETE CASCADE,
    CONSTRAINT FK_PrototypeParamSymbolRefs_Symbols_RefId FOREIGN KEY (RefId) REFERENCES Symbols (Id) ON DELETE NO ACTION
);

CREATE INDEX IX_ExampleCodeLines_ExampleId ON ExampleCodeLines (ExampleId);

CREATE INDEX IX_ExampleRequests_ApplyToId ON ExampleRequests (ApplyToId);

CREATE UNIQUE INDEX IX_ExampleRequests_DataId ON ExampleRequests (DataId); # MySQL does not permit non-nullable unique indices

CREATE INDEX IX_Examples_SymbolId ON Examples (SymbolId);

CREATE INDEX IX_Examples_UserId ON Examples (UserId);

CREATE INDEX IX_Permissions_GroupId ON Permissions (GroupId);

CREATE INDEX IX_PrototypeParams_PrototypeId ON PrototypeParams (PrototypeId);

CREATE UNIQUE INDEX IX_PrototypeParamSymbolRefs_PrototypeParamId ON PrototypeParamSymbolRefs (PrototypeParamId);

CREATE INDEX IX_PrototypeParamSymbolRefs_RefId ON PrototypeParamSymbolRefs (RefId);

CREATE INDEX IX_Prototypes_SymbolId ON Prototypes (SymbolId);

CREATE UNIQUE INDEX IX_SymbolLangs_Name ON SymbolLangs (Name); # MySQL does not permit non-nullable unique indices

CREATE INDEX IX_SymbolRefs_RefId ON SymbolRefs (RefId);

CREATE INDEX IX_SymbolRefs_SymbolId ON SymbolRefs (SymbolId);

CREATE INDEX IX_Symbols_ImportId ON Symbols (ImportId);

CREATE INDEX IX_Symbols_LangId ON Symbols (LangId);

CREATE INDEX IX_Symbols_LibId ON Symbols (LibId);

CREATE UNIQUE INDEX IX_Symbols_Path ON Symbols (Path); # MySQL does not permit non-nullable unique indices

CREATE INDEX IX_Symbols_TypeId ON Symbols (TypeId);

CREATE INDEX IX_Symbols_UserId ON Symbols (UserId);

CREATE UNIQUE INDEX IX_SymbolTypes_Name ON SymbolTypes (Name); # MySQL does not permit non-nullable unique indices

CREATE INDEX IX_Users_GroupId ON Users (GroupId);

INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
VALUES (N'20191031045835_Init', N'2.2.4-servicing-10062');
