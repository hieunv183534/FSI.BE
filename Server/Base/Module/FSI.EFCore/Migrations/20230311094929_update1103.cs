using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class update1103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "UserRoots",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoots_AccountId",
                table: "UserRoots",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoots_Accounts_Account~",
                table: "UserRoots",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoots_Accounts_Account~",
                table: "UserRoots");

            migrationBuilder.DropIndex(
                name: "IX_UserRoots_AccountId",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "UserRoots");
        }
    }
}
