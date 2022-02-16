using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_symbolinfo_to_t2trade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "T2SymbolInfoId",
                table: "T2Trades",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T2Trades_T2SymbolInfoId",
                table: "T2Trades",
                column: "T2SymbolInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_T2Trades_T2SymbolInfos_T2SymbolInfoId",
                table: "T2Trades",
                column: "T2SymbolInfoId",
                principalTable: "T2SymbolInfos",
                principalColumn: "T2SymbolInfoEntity_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2Trades_T2SymbolInfos_T2SymbolInfoId",
                table: "T2Trades");

            migrationBuilder.DropIndex(
                name: "IX_T2Trades_T2SymbolInfoId",
                table: "T2Trades");

            migrationBuilder.DropColumn(
                name: "T2SymbolInfoId",
                table: "T2Trades");
        }
    }
}
