using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class AddPositionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionID = table.Column<Guid>(type: "uuid", nullable: false),
                    PositionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EInvestmentType = table.Column<int>(type: "integer", nullable: false),
                    StockBroker = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    TicketCode = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    ISINCode = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    Bookkeeping = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "integer", nullable: false),
                    QuantityUnavailable = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    ClosingPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    ValueUpdated = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PositionID", x => x.PositionID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
