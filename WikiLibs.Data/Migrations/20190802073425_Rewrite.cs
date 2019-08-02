using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiLibs.Data.Migrations
{
    public partial class Rewrite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoTable");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_APIKeys",
                table: "APIKeys");

            migrationBuilder.DropColumn(
                name: "Lang",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "Lib",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Symbols");

            migrationBuilder.RenameTable(
                name: "APIKeys",
                newName: "ApiKeys");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Icon",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Symbols",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ImportId",
                table: "Symbols",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LangId",
                table: "Symbols",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LibId",
                table: "Symbols",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "Symbols",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Views",
                table: "Symbols",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "ApiKeys",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiKeys",
                table: "ApiKeys",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_SymbolImports_ImportId",
                table: "Symbols",
                column: "ImportId",
                principalTable: "SymbolImports",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_SymbolLangs_LangId",
                table: "Symbols",
                column: "LangId",
                principalTable: "SymbolLangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_SymbolLibs_LibId",
                table: "Symbols",
                column: "LibId",
                principalTable: "SymbolLibs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_SymbolTypes_TypeId",
                table: "Symbols",
                column: "TypeId",
                principalTable: "SymbolTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_SymbolImports_ImportId",
                table: "Symbols");

            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_SymbolLangs_LangId",
                table: "Symbols");

            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_SymbolLibs_LibId",
                table: "Symbols");

            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_SymbolTypes_TypeId",
                table: "Symbols");

            migrationBuilder.DropTable(
                name: "SymbolImports");

            migrationBuilder.DropTable(
                name: "SymbolLangs");

            migrationBuilder.DropTable(
                name: "SymbolLibs");

            migrationBuilder.DropTable(
                name: "SymbolTypes");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_ImportId",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_LangId",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_LibId",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_Path",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_TypeId",
                table: "Symbols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiKeys",
                table: "ApiKeys");

            migrationBuilder.DropColumn(
                name: "ImportId",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "LangId",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "LibId",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "ApiKeys");

            migrationBuilder.RenameTable(
                name: "ApiKeys",
                newName: "APIKeys");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Users",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Symbols",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lang",
                table: "Symbols",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lib",
                table: "Symbols",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Symbols",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_APIKeys",
                table: "APIKeys",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InfoTable",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoTable", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId",
                unique: true,
                filter: "[GroupId] IS NOT NULL");
        }
    }
}
