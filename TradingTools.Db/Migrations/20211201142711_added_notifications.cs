using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2PortfolioCoinEntity_T2SymbolInfos_T2SymbolInfoId",
                table: "T2PortfolioCoinEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_T2PortfolioCoinEntity_T2Syncs_T2SyncId",
                table: "T2PortfolioCoinEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T2PortfolioCoinEntity",
                table: "T2PortfolioCoinEntity");

            migrationBuilder.RenameTable(
                name: "T2PortfolioCoinEntity",
                newName: "T2PortfolioCoins");

            migrationBuilder.RenameIndex(
                name: "IX_T2PortfolioCoinEntity_T2SyncId",
                table: "T2PortfolioCoins",
                newName: "IX_T2PortfolioCoins_T2SyncId");

            migrationBuilder.RenameIndex(
                name: "IX_T2PortfolioCoinEntity_T2SymbolInfoId",
                table: "T2PortfolioCoins",
                newName: "IX_T2PortfolioCoins_T2SymbolInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T2PortfolioCoins",
                table: "T2PortfolioCoins",
                column: "T2PortfolioCoinEntity_ID");

            migrationBuilder.CreateTable(
                name: "T2Notifications",
                columns: table => new
                {
                    T2NotificationEntity_ID = table.Column<long>(type: "bigint", nullable: false, comment: "PK, Identity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of entity creation"),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of latest entity version"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T2Notifications", x => x.T2NotificationEntity_ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_T2PortfolioCoins_T2SymbolInfos_T2SymbolInfoId",
                table: "T2PortfolioCoins",
                column: "T2SymbolInfoId",
                principalTable: "T2SymbolInfos",
                principalColumn: "T2SymbolInfoEntity_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T2PortfolioCoins_T2Syncs_T2SyncId",
                table: "T2PortfolioCoins",
                column: "T2SyncId",
                principalTable: "T2Syncs",
                principalColumn: "T2SyncEntity_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T2PortfolioCoins_T2SymbolInfos_T2SymbolInfoId",
                table: "T2PortfolioCoins");

            migrationBuilder.DropForeignKey(
                name: "FK_T2PortfolioCoins_T2Syncs_T2SyncId",
                table: "T2PortfolioCoins");

            migrationBuilder.DropTable(
                name: "T2Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T2PortfolioCoins",
                table: "T2PortfolioCoins");

            migrationBuilder.RenameTable(
                name: "T2PortfolioCoins",
                newName: "T2PortfolioCoinEntity");

            migrationBuilder.RenameIndex(
                name: "IX_T2PortfolioCoins_T2SyncId",
                table: "T2PortfolioCoinEntity",
                newName: "IX_T2PortfolioCoinEntity_T2SyncId");

            migrationBuilder.RenameIndex(
                name: "IX_T2PortfolioCoins_T2SymbolInfoId",
                table: "T2PortfolioCoinEntity",
                newName: "IX_T2PortfolioCoinEntity_T2SymbolInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T2PortfolioCoinEntity",
                table: "T2PortfolioCoinEntity",
                column: "T2PortfolioCoinEntity_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_T2PortfolioCoinEntity_T2SymbolInfos_T2SymbolInfoId",
                table: "T2PortfolioCoinEntity",
                column: "T2SymbolInfoId",
                principalTable: "T2SymbolInfos",
                principalColumn: "T2SymbolInfoEntity_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T2PortfolioCoinEntity_T2Syncs_T2SyncId",
                table: "T2PortfolioCoinEntity",
                column: "T2SyncId",
                principalTable: "T2Syncs",
                principalColumn: "T2SyncEntity_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
