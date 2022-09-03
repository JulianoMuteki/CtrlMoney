using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class AddMovimentsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moviments",
                columns: table => new
                {
                    MovimentsID = table.Column<Guid>(type: "uuid", nullable: false),
                    InputOutput = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MovimentType = table.Column<string>(type: "text", nullable: true),
                    TicketCode = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    StockBroker = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    TransactionValue = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MovimentsID", x => x.MovimentsID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moviments");
        }
    }
}
