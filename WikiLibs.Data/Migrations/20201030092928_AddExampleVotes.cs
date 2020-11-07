using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace WikiLibs.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddExampleVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteCount",
                table: "Examples",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExampleVotes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    ExampleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleVotes", x => new { x.UserId, x.ExampleId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleVotes");

            migrationBuilder.DropColumn(
                name: "VoteCount",
                table: "Examples");
        }
    }
}
