using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class TreeViewBootstrap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrandChildrenTrees",
                columns: table => new
                {
                    GrandChildTreeID = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    IsDisable = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    LevelTree = table.Column<int>(nullable: false),
                    ParentNodeID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GrandChildTreeID", x => x.GrandChildTreeID);
                });

            migrationBuilder.CreateTable(
                name: "ParentsTrees",
                columns: table => new
                {
                    ParentTreeID = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    IsDisable = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    LevelTree = table.Column<int>(nullable: false),
                    ParentNodeID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ParentTreeID", x => x.ParentTreeID);
                    table.ForeignKey(
                        name: "FK_ParentsTrees_GrandChildrenTrees_ParentNodeID",
                        column: x => x.ParentNodeID,
                        principalTable: "GrandChildrenTrees",
                        principalColumn: "GrandChildTreeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChildrenTrees",
                columns: table => new
                {
                    ChildTreeID = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    IsDisable = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    LevelTree = table.Column<int>(nullable: false),
                    ParentNodeID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ChildTreeID", x => x.ChildTreeID);
                    table.ForeignKey(
                        name: "FK_ChildrenTrees_ParentsTrees_ParentNodeID",
                        column: x => x.ParentNodeID,
                        principalTable: "ParentsTrees",
                        principalColumn: "ParentTreeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildrenTrees_ParentNodeID",
                table: "ChildrenTrees",
                column: "ParentNodeID");

            migrationBuilder.CreateIndex(
                name: "IX_GrandChildrenTrees_ParentNodeID",
                table: "GrandChildrenTrees",
                column: "ParentNodeID");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsTrees_ParentNodeID",
                table: "ParentsTrees",
                column: "ParentNodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_GrandChildrenTrees_ChildrenTrees_ParentNodeID",
                table: "GrandChildrenTrees",
                column: "ParentNodeID",
                principalTable: "ChildrenTrees",
                principalColumn: "ChildTreeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenTrees_ParentsTrees_ParentNodeID",
                table: "ChildrenTrees");

            migrationBuilder.DropTable(
                name: "ParentsTrees");

            migrationBuilder.DropTable(
                name: "GrandChildrenTrees");

            migrationBuilder.DropTable(
                name: "ChildrenTrees");
        }
    }
}
