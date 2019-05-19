using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiLibs.Data.Migrations
{
    public partial class AddExamples : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examples_Symbols_SymbolId",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Examples");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolId",
                table: "Examples",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Examples",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExampleCodeLines",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExampleId = table.Column<long>(nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExampleCodeLines_ExampleId",
                table: "ExampleCodeLines",
                column: "ExampleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Examples_Symbols_SymbolId",
                table: "Examples",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examples_Symbols_SymbolId",
                table: "Examples");

            migrationBuilder.DropTable(
                name: "ExampleCodeLines");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Examples");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolId",
                table: "Examples",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Examples",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Examples_Symbols_SymbolId",
                table: "Examples",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
