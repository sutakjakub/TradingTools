using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_nullable_for_tradegroupid_in_orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2Orders_T2TradeGroups_T2TradeGroupId",
                table: "T2Orders");

            migrationBuilder.AlterColumn<long>(
                name: "T2TradeGroupId",
                table: "T2Orders",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_T2Orders_T2TradeGroups_T2TradeGroupId",
                table: "T2Orders",
                column: "T2TradeGroupId",
                principalTable: "T2TradeGroups",
                principalColumn: "T2TradeGroupEntity_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2Orders_T2TradeGroups_T2TradeGroupId",
                table: "T2Orders");

            migrationBuilder.AlterColumn<long>(
                name: "T2TradeGroupId",
                table: "T2Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_T2Orders_T2TradeGroups_T2TradeGroupId",
                table: "T2Orders",
                column: "T2TradeGroupId",
                principalTable: "T2TradeGroups",
                principalColumn: "T2TradeGroupEntity_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
