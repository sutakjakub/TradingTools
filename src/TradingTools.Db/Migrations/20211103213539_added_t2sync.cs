using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingTools.Db.Migrations
{
    public partial class added_t2sync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T2Syncs",
                columns: table => new
                {
                    T2SyncEntity_ID = table.Column<long>(type: "bigint", nullable: false, comment: "PK, Identity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of entity creation"),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset(2)", nullable: false, comment: "Represents UTC date time of latest entity version"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T2Syncs", x => x.T2SyncEntity_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T2Syncs");
        }
    }
}
