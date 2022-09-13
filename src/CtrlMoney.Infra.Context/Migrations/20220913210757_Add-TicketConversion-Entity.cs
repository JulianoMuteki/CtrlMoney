using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class AddTicketConversionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketsConversions",
                columns: table => new
                {
                    TicketConversionID = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketInput = table.Column<string>(type: "varchar", maxLength: 15, nullable: false),
                    TicketOutput = table.Column<string>(type: "varchar", maxLength: 15, nullable: false),
                    StockBroker = table.Column<string>(type: "varchar", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TicketConversionID", x => x.TicketConversionID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketsConversions");
        }
    }
}
