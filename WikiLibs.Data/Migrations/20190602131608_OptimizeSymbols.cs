using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace WikiLibs.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class OptimizeSymbols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "PrototypeParams");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "SymbolRefs",
                newName: "RefPath");

            migrationBuilder.AddColumn<string>(
                name: "Lib",
                table: "Symbols",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RefId",
                table: "SymbolRefs",
                nullable: true);

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
                name: "IX_SymbolRefs_RefId",
                table: "SymbolRefs",
                column: "RefId");

            migrationBuilder.CreateIndex(
                name: "IX_PrototypeParamSymbolRefs_PrototypeParamId",
                table: "PrototypeParamSymbolRefs",
                column: "PrototypeParamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrototypeParamSymbolRefs_RefId",
                table: "PrototypeParamSymbolRefs",
                column: "RefId");

            migrationBuilder.AddForeignKey(
                name: "FK_SymbolRefs_Symbols_RefId",
                table: "SymbolRefs",
                column: "RefId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SymbolRefs_Symbols_RefId",
                table: "SymbolRefs");

            migrationBuilder.DropTable(
                name: "PrototypeParamSymbolRefs");

            migrationBuilder.DropIndex(
                name: "IX_SymbolRefs_RefId",
                table: "SymbolRefs");

            migrationBuilder.DropColumn(
                name: "Lib",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "RefId",
                table: "SymbolRefs");

            migrationBuilder.RenameColumn(
                name: "RefPath",
                table: "SymbolRefs",
                newName: "Path");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "PrototypeParams",
                nullable: true);
        }
    }
}
