using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_tradegroup_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "QuoteUsdValue",
                table: "T2Trades",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<long>(
                name: "T2TradeGroupEntityId",
                table: "T2Trades",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "T2TradeGroups",
                columns: table => new
                {
                    T2TradeGroupEntity_ID = table.Column<long>(type: "bigint", nullable: false, comment: "PK, Identity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SymbolInfoId = table.Column<long>(type: "bigint", nullable: false),
                    BaseAsset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of entity creation"),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of latest entity version"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T2TradeGroups", x => x.T2TradeGroupEntity_ID);
                    table.ForeignKey(
                        name: "FK_T2TradeGroups_T2SymbolInfos_SymbolInfoId",
                        column: x => x.SymbolInfoId,
                        principalTable: "T2SymbolInfos",
                        principalColumn: "T2SymbolInfoEntity_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T2Trades_T2TradeGroupEntityId",
                table: "T2Trades",
                column: "T2TradeGroupEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_T2TradeGroups_SymbolInfoId",
                table: "T2TradeGroups",
                column: "SymbolInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_T2Trades_T2TradeGroups_T2TradeGroupEntityId",
                table: "T2Trades",
                column: "T2TradeGroupEntityId",
                principalTable: "T2TradeGroups",
                principalColumn: "T2TradeGroupEntity_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2Trades_T2TradeGroups_T2TradeGroupEntityId",
                table: "T2Trades");

            migrationBuilder.DropTable(
                name: "T2TradeGroups");

            migrationBuilder.DropIndex(
                name: "IX_T2Trades_T2TradeGroupEntityId",
                table: "T2Trades");

            migrationBuilder.DropColumn(
                name: "T2TradeGroupEntityId",
                table: "T2Trades");

            migrationBuilder.AlterColumn<decimal>(
                name: "QuoteUsdValue",
                table: "T2Trades",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,8)",
                oldPrecision: 18,
                oldScale: 8);
        }
    }
}
