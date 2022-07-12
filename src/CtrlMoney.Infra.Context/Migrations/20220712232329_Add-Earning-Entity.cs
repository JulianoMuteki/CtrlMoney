using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class AddEarningEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Earnings",
                columns: table => new
                {
                    EarningID = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketCode = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    EventType = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    StockBroker = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    NetValue = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EarningID", x => x.EarningID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Earnings");
        }
    }
}
