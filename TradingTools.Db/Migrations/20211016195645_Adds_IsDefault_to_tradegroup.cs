using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class Adds_IsDefault_to_tradegroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "T2TradeGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "T2TradeGroups");
        }
    }
}
