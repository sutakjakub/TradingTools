using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class tradegtoup_symbolInfoId_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2TradeGroups_T2SymbolInfos_SymbolInfoId",
                table: "T2TradeGroups");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolInfoId",
                table: "T2TradeGroups",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_T2TradeGroups_T2SymbolInfos_SymbolInfoId",
                table: "T2TradeGroups",
                column: "SymbolInfoId",
                principalTable: "T2SymbolInfos",
                principalColumn: "T2SymbolInfoEntity_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2TradeGroups_T2SymbolInfos_SymbolInfoId",
                table: "T2TradeGroups");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolInfoId",
                table: "T2TradeGroups",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_T2TradeGroups_T2SymbolInfos_SymbolInfoId",
                table: "T2TradeGroups",
                column: "SymbolInfoId",
                principalTable: "T2SymbolInfos",
                principalColumn: "T2SymbolInfoEntity_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
