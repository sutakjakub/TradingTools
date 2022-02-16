using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_relation_between_trade_and_group : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "T2TradeGroupId",
                table: "T2Trades",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T2Trades_T2TradeGroupId",
                table: "T2Trades",
                column: "T2TradeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_T2Trades_T2TradeGroups_T2TradeGroupId",
                table: "T2Trades",
                column: "T2TradeGroupId",
                principalTable: "T2TradeGroups",
                principalColumn: "T2TradeGroupEntity_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2Trades_T2TradeGroups_T2TradeGroupId",
                table: "T2Trades");

            migrationBuilder.DropIndex(
                name: "IX_T2Trades_T2TradeGroupId",
                table: "T2Trades");

            migrationBuilder.DropColumn(
                name: "T2TradeGroupId",
                table: "T2Trades");
        }
    }
}
