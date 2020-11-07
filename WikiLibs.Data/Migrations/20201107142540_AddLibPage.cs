using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiLibs.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddLibPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Copyright",
                table: "SymbolLibs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SymbolLibs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "SymbolLibs",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Icon",
                table: "SymbolLibs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SymbolLibs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SymbolLibs_UserId",
                table: "SymbolLibs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SymbolLibs_Users_UserId",
                table: "SymbolLibs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SymbolLibs_Users_UserId",
                table: "SymbolLibs");

            migrationBuilder.DropIndex(
                name: "IX_SymbolLibs_UserId",
                table: "SymbolLibs");

            migrationBuilder.DropColumn(
                name: "Copyright",
                table: "SymbolLibs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SymbolLibs");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "SymbolLibs");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "SymbolLibs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SymbolLibs");
        }
    }
}
