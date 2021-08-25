using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class make_nullable_T2OrderId_in_T2Trade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2Trades_T2Orders_T2OrderId",
                table: "T2Trades");

            migrationBuilder.AlterColumn<long>(
                name: "T2OrderId",
                table: "T2Trades",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_T2Trades_T2Orders_T2OrderId",
                table: "T2Trades",
                column: "T2OrderId",
                principalTable: "T2Orders",
                principalColumn: "T2OrderEntity_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2Trades_T2Orders_T2OrderId",
                table: "T2Trades");

            migrationBuilder.AlterColumn<long>(
                name: "T2OrderId",
                table: "T2Trades",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_T2Trades_T2Orders_T2OrderId",
                table: "T2Trades",
                column: "T2OrderId",
                principalTable: "T2Orders",
                principalColumn: "T2OrderEntity_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
