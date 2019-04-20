using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiLibs.Data.Migrations
{
    public partial class APIKeySecurity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "APIKeys",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Flags",
                table: "APIKeys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UseNum",
                table: "APIKeys",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "APIKeys");

            migrationBuilder.DropColumn(
                name: "Flags",
                table: "APIKeys");

            migrationBuilder.DropColumn(
                name: "UseNum",
                table: "APIKeys");
        }
    }
}
