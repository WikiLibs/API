using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiLibs.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Flags = table.Column<int>(nullable: false),
                    UseNum = table.Column<int>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    Origin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymbolImports",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolImports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymbolLangs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolLangs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymbolLibs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolLibs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymbolTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<long>(nullable: false),
                    Perm = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Icon = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Confirmation = table.Column<string>(nullable: true),
                    Private = table.Column<bool>(nullable: false),
                    ProfileMsg = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    Pseudo = table.Column<string>(nullable: true),
                    GroupId = table.Column<long>(nullable: true),
                    Pass = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    IsBot = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Path = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    LangId = table.Column<long>(nullable: false),
                    LibId = table.Column<long>(nullable: false),
                    TypeId = table.Column<long>(nullable: false),
                    ImportId = table.Column<long>(nullable: true),
                    Views = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastModificationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Symbols_SymbolImports_ImportId",
                        column: x => x.ImportId,
                        principalTable: "SymbolImports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Symbols_SymbolLangs_LangId",
                        column: x => x.LangId,
                        principalTable: "SymbolLangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Symbols_SymbolLibs_LibId",
                        column: x => x.LibId,
                        principalTable: "SymbolLibs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Symbols_SymbolTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SymbolTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Symbols_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Examples",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SymbolId = table.Column<long>(nullable: false),
                    RequestId = table.Column<long>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastModificationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Examples_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Examples_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Prototypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SymbolId = table.Column<long>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prototypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prototypes_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymbolRefs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SymbolId = table.Column<long>(nullable: false),
                    RefId = table.Column<long>(nullable: true),
                    RefPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolRefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymbolRefs_Symbols_RefId",
                        column: x => x.RefId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SymbolRefs_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExampleCodeLines",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExampleId = table.Column<long>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleCodeLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleCodeLines_Examples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "Examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExampleRequests",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataId = table.Column<long>(nullable: true),
                    ApplyToId = table.Column<long>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleRequests_Examples_ApplyToId",
                        column: x => x.ApplyToId,
                        principalTable: "Examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExampleRequests_Examples_DataId",
                        column: x => x.DataId,
                        principalTable: "Examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PrototypeParams",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrototypeId = table.Column<long>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrototypeParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrototypeParams_Prototypes_PrototypeId",
                        column: x => x.PrototypeId,
                        principalTable: "Prototypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrototypeParamSymbolRefs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrototypeParamId = table.Column<long>(nullable: false),
                    RefId = table.Column<long>(nullable: true),
                    RefPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrototypeParamSymbolRefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrototypeParamSymbolRefs_PrototypeParams_PrototypeParamId",
                        column: x => x.PrototypeParamId,
                        principalTable: "PrototypeParams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrototypeParamSymbolRefs_Symbols_RefId",
                        column: x => x.RefId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExampleCodeLines_ExampleId",
                table: "ExampleCodeLines",
                column: "ExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleRequests_ApplyToId",
                table: "ExampleRequests",
                column: "ApplyToId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleRequests_DataId",
                table: "ExampleRequests",
                column: "DataId",
                unique: true,
                filter: "[DataId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Examples_SymbolId",
                table: "Examples",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Examples_UserId",
                table: "Examples",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_GroupId",
                table: "Permissions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PrototypeParams_PrototypeId",
                table: "PrototypeParams",
                column: "PrototypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PrototypeParamSymbolRefs_PrototypeParamId",
                table: "PrototypeParamSymbolRefs",
                column: "PrototypeParamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrototypeParamSymbolRefs_RefId",
                table: "PrototypeParamSymbolRefs",
                column: "RefId");

            migrationBuilder.CreateIndex(
                name: "IX_Prototypes_SymbolId",
                table: "Prototypes",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolRefs_RefId",
                table: "SymbolRefs",
                column: "RefId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolRefs_SymbolId",
                table: "SymbolRefs",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_ImportId",
                table: "Symbols",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_LangId",
                table: "Symbols",
                column: "LangId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_LibId",
                table: "Symbols",
                column: "LibId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Path",
                table: "Symbols",
                column: "Path",
                unique: true,
                filter: "[Path] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_TypeId",
                table: "Symbols",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_UserId",
                table: "Symbols",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "ExampleCodeLines");

            migrationBuilder.DropTable(
                name: "ExampleRequests");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PrototypeParamSymbolRefs");

            migrationBuilder.DropTable(
                name: "SymbolRefs");

            migrationBuilder.DropTable(
                name: "Examples");

            migrationBuilder.DropTable(
                name: "PrototypeParams");

            migrationBuilder.DropTable(
                name: "Prototypes");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropTable(
                name: "SymbolImports");

            migrationBuilder.DropTable(
                name: "SymbolLangs");

            migrationBuilder.DropTable(
                name: "SymbolLibs");

            migrationBuilder.DropTable(
                name: "SymbolTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
