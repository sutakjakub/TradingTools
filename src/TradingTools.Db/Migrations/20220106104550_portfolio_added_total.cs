using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class portfolio_added_total : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TotalDollarAssetName",
                table: "T2PortfolioCoins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDollarValue",
                table: "T2PortfolioCoins",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TotalQuoteAssetName",
                table: "T2PortfolioCoins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalQuoteValue",
                table: "T2PortfolioCoins",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDollarAssetName",
                table: "T2PortfolioCoins");

            migrationBuilder.DropColumn(
                name: "TotalDollarValue",
                table: "T2PortfolioCoins");

            migrationBuilder.DropColumn(
                name: "TotalQuoteAssetName",
                table: "T2PortfolioCoins");

            migrationBuilder.DropColumn(
                name: "TotalQuoteValue",
                table: "T2PortfolioCoins");
        }
    }
}
