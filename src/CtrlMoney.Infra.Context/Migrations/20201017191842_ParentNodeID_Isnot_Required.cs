using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CtrlMoney.Infra.Context.Migrations
{
    public partial class ParentNodeID_Isnot_Required : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenTrees_ParentsTrees_ParentNodeID",
                table: "ChildrenTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_GrandChildrenTrees_ChildrenTrees_ParentNodeID",
                table: "GrandChildrenTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentsTrees_GrandChildrenTrees_ParentNodeID",
                table: "ParentsTrees");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentNodeID",
                table: "ParentsTrees",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentNodeID",
                table: "GrandChildrenTrees",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentNodeID",
                table: "ChildrenTrees",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenTrees_ParentsTrees_ParentNodeID",
                table: "ChildrenTrees",
                column: "ParentNodeID",
                principalTable: "ParentsTrees",
                principalColumn: "ParentTreeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GrandChildrenTrees_ChildrenTrees_ParentNodeID",
                table: "GrandChildrenTrees",
                column: "ParentNodeID",
                principalTable: "ChildrenTrees",
                principalColumn: "ChildTreeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentsTrees_GrandChildrenTrees_ParentNodeID",
                table: "ParentsTrees",
                column: "ParentNodeID",
                principalTable: "GrandChildrenTrees",
                principalColumn: "GrandChildTreeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenTrees_ParentsTrees_ParentNodeID",
                table: "ChildrenTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_GrandChildrenTrees_ChildrenTrees_ParentNodeID",
                table: "GrandChildrenTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentsTrees_GrandChildrenTrees_ParentNodeID",
                table: "ParentsTrees");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentNodeID",
                table: "ParentsTrees",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentNodeID",
                table: "GrandChildrenTrees",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentNodeID",
                table: "ChildrenTrees",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenTrees_ParentsTrees_ParentNodeID",
                table: "ChildrenTrees",
                column: "ParentNodeID",
                principalTable: "ParentsTrees",
                principalColumn: "ParentTreeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GrandChildrenTrees_ChildrenTrees_ParentNodeID",
                table: "GrandChildrenTrees",
                column: "ParentNodeID",
                principalTable: "ChildrenTrees",
                principalColumn: "ChildTreeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentsTrees_GrandChildrenTrees_ParentNodeID",
                table: "ParentsTrees",
                column: "ParentNodeID",
                principalTable: "GrandChildrenTrees",
                principalColumn: "GrandChildTreeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
