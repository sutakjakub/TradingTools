using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_portfolio_coin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T2PortfolioCoinEntity",
                columns: table => new
                {
                    T2PortfolioCoinEntity_ID = table.Column<long>(type: "bigint", nullable: false, comment: "PK, Identity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    T2SyncId = table.Column<long>(type: "bigint", nullable: false),
                    T2SymbolInfoId = table.Column<long>(type: "bigint", nullable: false),
                    Free = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    Locked = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of entity creation"),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of latest entity version"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T2PortfolioCoinEntity", x => x.T2PortfolioCoinEntity_ID);
                    table.ForeignKey(
                        name: "FK_T2PortfolioCoinEntity_T2SymbolInfos_T2SymbolInfoId",
                        column: x => x.T2SymbolInfoId,
                        principalTable: "T2SymbolInfos",
                        principalColumn: "T2SymbolInfoEntity_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T2PortfolioCoinEntity_T2Syncs_T2SyncId",
                        column: x => x.T2SyncId,
                        principalTable: "T2Syncs",
                        principalColumn: "T2SyncEntity_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T2PortfolioCoinEntity_T2SymbolInfoId",
                table: "T2PortfolioCoinEntity",
                column: "T2SymbolInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_T2PortfolioCoinEntity_T2SyncId",
                table: "T2PortfolioCoinEntity",
                column: "T2SyncId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T2PortfolioCoinEntity");
        }
    }
}
