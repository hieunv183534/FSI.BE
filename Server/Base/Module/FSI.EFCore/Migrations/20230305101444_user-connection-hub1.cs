using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class userconnectionhub1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_Conversati~",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_Conversati~",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "UserConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "UserConnections",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ConnectionId",
                table: "UserConnections",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "UserConnections",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_Conversati~",
                table: "UserConnections",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_Conversati~",
                table: "UserConnections",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
