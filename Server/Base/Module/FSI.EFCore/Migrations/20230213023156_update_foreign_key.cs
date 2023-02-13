using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class update_foreign_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserConversations_Conversa~",
                table: "UserConversations",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConversations_UserId",
                table: "UserConversations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_AuthorId",
                table: "Files",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_UserRoots_AuthorId",
                table: "Files",
                column: "AuthorId",
                principalTable: "UserRoots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_Con~",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserRoots_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "UserRoots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_Conversa~",
                table: "UserConversations",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_UserRoot~",
                table: "UserConversations",
                column: "UserId",
                principalTable: "UserRoots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_UserRoots_AuthorId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_Con~",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserRoots_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_Conversa~",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_UserRoot~",
                table: "UserConversations");

            migrationBuilder.DropIndex(
                name: "IX_UserConversations_Conversa~",
                table: "UserConversations");

            migrationBuilder.DropIndex(
                name: "IX_UserConversations_UserId",
                table: "UserConversations");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Files_AuthorId",
                table: "Files");
        }
    }
}
