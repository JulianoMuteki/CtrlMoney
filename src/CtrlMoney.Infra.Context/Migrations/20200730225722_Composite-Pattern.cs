using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class CompositePattern : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Composites");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Leaves",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "LeafParentID",
                table: "Composites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Composites",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Composites_LeafParentID",
                table: "Composites",
                column: "LeafParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Composites_Leaves_LeafParentID",
                table: "Composites",
                column: "LeafParentID",
                principalTable: "Leaves",
                principalColumn: "LeafID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Composites_Leaves_LeafParentID",
                table: "Composites");

            migrationBuilder.DropIndex(
                name: "IX_Composites_LeafParentID",
                table: "Composites");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "LeafParentID",
                table: "Composites");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Composites");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Leaves",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Composites",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }
    }
}
