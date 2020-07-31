using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class CompositeGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    LeafID = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    IsDisable = table.Column<bool>(nullable: false),
                    CompositeID = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LeafID", x => x.LeafID);
                });

            migrationBuilder.CreateTable(
                name: "Composites",
                columns: table => new
                {
                    CompositeID = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    IsDisable = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    LeafParentID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CompositeID", x => x.CompositeID);
                    table.ForeignKey(
                        name: "FK_Composites_Leaves_LeafParentID",
                        column: x => x.LeafParentID,
                        principalTable: "Leaves",
                        principalColumn: "LeafID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Composites_LeafParentID",
                table: "Composites",
                column: "LeafParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_CompositeID",
                table: "Leaves",
                column: "CompositeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Composites_CompositeID",
                table: "Leaves",
                column: "CompositeID",
                principalTable: "Composites",
                principalColumn: "CompositeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Composites_Leaves_LeafParentID",
                table: "Composites");

            migrationBuilder.DropTable(
                name: "Leaves");

            migrationBuilder.DropTable(
                name: "Composites");
        }
    }
}
