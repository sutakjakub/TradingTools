using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class portfolio_removed_symbolInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2PortfolioCoins_T2SymbolInfos_T2SymbolInfoId",
                table: "T2PortfolioCoins");

            migrationBuilder.DropIndex(
                name: "IX_T2PortfolioCoins_T2SymbolInfoId",
                table: "T2PortfolioCoins");

            migrationBuilder.DropColumn(
                name: "T2SymbolInfoId",
                table: "T2PortfolioCoins");

            migrationBuilder.AddColumn<string>(
                name: "Coin",
                table: "T2PortfolioCoins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoinName",
                table: "T2PortfolioCoins",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coin",
                table: "T2PortfolioCoins");

            migrationBuilder.DropColumn(
                name: "CoinName",
                table: "T2PortfolioCoins");

            migrationBuilder.AddColumn<long>(
                name: "T2SymbolInfoId",
                table: "T2PortfolioCoins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_T2PortfolioCoins_T2SymbolInfoId",
                table: "T2PortfolioCoins",
                column: "T2SymbolInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_T2PortfolioCoins_T2SymbolInfos_T2SymbolInfoId",
                table: "T2PortfolioCoins",
                column: "T2SymbolInfoId",
                principalTable: "T2SymbolInfos",
                principalColumn: "T2SymbolInfoEntity_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
