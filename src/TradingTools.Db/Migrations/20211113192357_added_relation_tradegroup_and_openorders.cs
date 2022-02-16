using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_relation_tradegroup_and_openorders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "T2TradeGroupId",
                table: "T2Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_T2Orders_T2TradeGroupId",
                table: "T2Orders",
                column: "T2TradeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_T2Orders_T2TradeGroups_T2TradeGroupId",
                table: "T2Orders",
                column: "T2TradeGroupId",
                principalTable: "T2TradeGroups",
                principalColumn: "T2TradeGroupEntity_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2Orders_T2TradeGroups_T2TradeGroupId",
                table: "T2Orders");

            migrationBuilder.DropIndex(
                name: "IX_T2Orders_T2TradeGroupId",
                table: "T2Orders");

            migrationBuilder.DropColumn(
                name: "T2TradeGroupId",
                table: "T2Orders");
        }
    }
}
