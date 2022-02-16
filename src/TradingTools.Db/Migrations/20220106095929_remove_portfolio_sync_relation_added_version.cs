using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class remove_portfolio_sync_relation_added_version : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2PortfolioCoins_T2Syncs_T2SyncId",
                table: "T2PortfolioCoins");

            migrationBuilder.DropIndex(
                name: "IX_T2PortfolioCoins_T2SyncId",
                table: "T2PortfolioCoins");

            migrationBuilder.DropColumn(
                name: "T2SyncId",
                table: "T2PortfolioCoins");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "T2PortfolioCoins",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "T2PortfolioCoins");

            migrationBuilder.AddColumn<long>(
                name: "T2SyncId",
                table: "T2PortfolioCoins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_T2PortfolioCoins_T2SyncId",
                table: "T2PortfolioCoins",
                column: "T2SyncId");

            migrationBuilder.AddForeignKey(
                name: "FK_T2PortfolioCoins_T2Syncs_T2SyncId",
                table: "T2PortfolioCoins",
                column: "T2SyncId",
                principalTable: "T2Syncs",
                principalColumn: "T2SyncEntity_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
