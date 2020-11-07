using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace WikiLibs.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddLibPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
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
                name: "DisplayName",
                table: "SymbolLibs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SymbolLibs");
        }
    }
}
