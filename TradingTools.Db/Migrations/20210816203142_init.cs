using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T2SymbolInfos",
                columns: table => new
                {
                    T2SymbolInfoEntity_ID = table.Column<long>(type: "bigint", nullable: false, comment: "PK, Identity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExchangeType = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseAsset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseAssetPrecision = table.Column<int>(type: "int", nullable: false),
                    BaseAssetCommissionPrecision = table.Column<int>(type: "int", nullable: false),
                    QuoteAsset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteAssetPrecision = table.Column<int>(type: "int", nullable: false),
                    QuoteAssetCommissionPrecision = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of entity creation"),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of latest entity version"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T2SymbolInfos", x => x.T2SymbolInfoEntity_ID);
                });

            migrationBuilder.CreateTable(
                name: "T2Orders",
                columns: table => new
                {
                    T2OrderEntity_ID = table.Column<long>(type: "bigint", nullable: false, comment: "PK, Identity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    T2SymbolInfoId = table.Column<long>(type: "bigint", nullable: false),
                    IsWorking = table.Column<bool>(type: "bit", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StopPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Side = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TimeInForce = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    QuoteQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuoteQuantityFilled = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityFilled = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalClientOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderListId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of entity creation"),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of latest entity version"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T2Orders", x => x.T2OrderEntity_ID);
                    table.ForeignKey(
                        name: "FK_T2Orders_T2SymbolInfos_T2SymbolInfoId",
                        column: x => x.T2SymbolInfoId,
                        principalTable: "T2SymbolInfos",
                        principalColumn: "T2SymbolInfoEntity_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T2Trades",
                columns: table => new
                {
                    T2TradeEntity_ID = table.Column<long>(type: "bigint", nullable: false, comment: "PK, Identity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    T2TradeId = table.Column<long>(type: "bigint", nullable: false),
                    T2OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ExchangeType = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuoteQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Commission = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommissionAsset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of entity creation"),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of latest entity version"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T2Trades", x => x.T2TradeEntity_ID);
                    table.ForeignKey(
                        name: "FK_T2Trades_T2Orders_T2OrderId",
                        column: x => x.T2OrderId,
                        principalTable: "T2Orders",
                        principalColumn: "T2OrderEntity_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T2Orders_T2SymbolInfoId",
                table: "T2Orders",
                column: "T2SymbolInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_T2Trades_T2OrderId",
                table: "T2Trades",
                column: "T2OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T2Trades");

            migrationBuilder.DropTable(
                name: "T2Orders");

            migrationBuilder.DropTable(
                name: "T2SymbolInfos");
        }
    }
}
