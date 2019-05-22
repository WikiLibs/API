using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiLibs.Data.Migrations
{
    public partial class AddExamples : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_Users_UserId",
                table: "Symbols");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_UserId",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Examples");

            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                table: "Examples",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId",
                unique: true,
                filter: "[GroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_UserId",
                table: "Symbols",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleCodeLines_ExampleId",
                table: "ExampleCodeLines",
                column: "ExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleRequests_ApplyToId",
                table: "ExampleRequests",
                column: "ApplyToId",
                unique: true,
                filter: "[ApplyToId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleRequests_DataId",
                table: "ExampleRequests",
                column: "DataId",
                unique: true,
                filter: "[DataId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_Users_UserId",
                table: "Symbols",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_Users_UserId",
                table: "Symbols");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ExampleCodeLines");

            migrationBuilder.DropTable(
                name: "ExampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_UserId",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Examples");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Examples",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_UserId",
                table: "Symbols",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_Users_UserId",
                table: "Symbols",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
