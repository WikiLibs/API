using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiLibs.Data.Migrations
{
    public partial class DbRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examples_Users_UserUUID",
                table: "Examples");

            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_Users_UserUUID",
                table: "Symbols");

            migrationBuilder.RenameColumn(
                name: "UUID",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserUUID",
                table: "Symbols",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Symbols_UserUUID",
                table: "Symbols",
                newName: "IX_Symbols_UserId");

            migrationBuilder.RenameColumn(
                name: "UserUUID",
                table: "Examples",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Examples_UserUUID",
                table: "Examples",
                newName: "IX_Examples_UserId");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "APIKeys",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Examples_Users_UserId",
                table: "Examples",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_Users_UserId",
                table: "Symbols",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examples_Users_UserId",
                table: "Examples");

            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_Users_UserId",
                table: "Symbols");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UUID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Symbols",
                newName: "UserUUID");

            migrationBuilder.RenameIndex(
                name: "IX_Symbols_UserId",
                table: "Symbols",
                newName: "IX_Symbols_UserUUID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Examples",
                newName: "UserUUID");

            migrationBuilder.RenameIndex(
                name: "IX_Examples_UserId",
                table: "Examples",
                newName: "IX_Examples_UserUUID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "APIKeys",
                newName: "Key");

            migrationBuilder.AddForeignKey(
                name: "FK_Examples_Users_UserUUID",
                table: "Examples",
                column: "UserUUID",
                principalTable: "Users",
                principalColumn: "UUID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_Users_UserUUID",
                table: "Symbols",
                column: "UserUUID",
                principalTable: "Users",
                principalColumn: "UUID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
