using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_props_to_trade_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBestMatch",
                table: "T2Trades",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyer",
                table: "T2Trades",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaker",
                table: "T2Trades",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBestMatch",
                table: "T2Trades");

            migrationBuilder.DropColumn(
                name: "IsBuyer",
                table: "T2Trades");

            migrationBuilder.DropColumn(
                name: "IsMaker",
                table: "T2Trades");
        }
    }
}
