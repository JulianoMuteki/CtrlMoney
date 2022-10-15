using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class AddStatusInvestFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Brokerage",
                table: "BrokeragesHistories",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "BrokeragesHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Fees",
                table: "BrokeragesHistories",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IRRF",
                table: "BrokeragesHistories",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Taxes",
                table: "BrokeragesHistories",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brokerage",
                table: "BrokeragesHistories");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "BrokeragesHistories");

            migrationBuilder.DropColumn(
                name: "Fees",
                table: "BrokeragesHistories");

            migrationBuilder.DropColumn(
                name: "IRRF",
                table: "BrokeragesHistories");

            migrationBuilder.DropColumn(
                name: "Taxes",
                table: "BrokeragesHistories");
        }
    }
}
