using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiLibs.Data.Migrations
{
    public partial class AddExamples : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Examples");

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
                column: "DataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleCodeLines");

            migrationBuilder.DropTable(
                name: "ExampleRequests");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Examples",
                nullable: true);
        }
    }
}
