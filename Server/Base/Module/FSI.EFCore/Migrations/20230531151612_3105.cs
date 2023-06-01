using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class _3105 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Field",
                table: "Projects",
                newName: "Fields");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "UserRoots",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Projects",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "FounderId",
                table: "Projects",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FounderId",
                table: "Projects",
                column: "FounderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_UserRoots_FounderId",
                table: "Projects",
                column: "FounderId",
                principalTable: "UserRoots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_UserRoots_FounderId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FounderId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FounderId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "Fields",
                table: "Projects",
                newName: "Field");
        }
    }
}
