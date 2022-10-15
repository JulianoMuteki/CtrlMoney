using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class ChangeEarningsToStatusInvest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "Earnings",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "NetValue",
                table: "Earnings",
                newName: "TotalNetAmount");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Earnings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Earnings",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "WithDate",
                table: "Earnings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Earnings");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Earnings");

            migrationBuilder.DropColumn(
                name: "WithDate",
                table: "Earnings");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Earnings",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "TotalNetAmount",
                table: "Earnings",
                newName: "NetValue");
        }
    }
}
