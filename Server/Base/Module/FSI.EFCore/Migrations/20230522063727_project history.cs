using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class projecthistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_UserRoots_U~1",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_UserRoots_Us~",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_Con~",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_Projects_Proj~",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_UserRoots_Use~",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_UserRoots_~",
                table: "UserConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_Conversa~",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_UserRoot~",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoots_Accounts_Account~",
                table: "UserRoots");

            migrationBuilder.RenameIndex(
                name: "IX_UserConversations_Conversa~",
                table: "UserConversations",
                newName: "IX_UserConversations_ConversationId");

            migrationBuilder.RenameColumn(
                name: "GrowHistory",
                table: "Projects",
                newName: "History");

            migrationBuilder.AddColumn<int>(
                name: "TotalExpectedInvestment",
                table: "ProjectUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalInvestment",
                table: "ProjectUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_UserRoots_UserAId",
                table: "Conversations",
                column: "UserAId",
                principalTable: "UserRoots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_UserRoots_UserBId",
                table: "Conversations",
                column: "UserBId",
                principalTable: "UserRoots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_Projects_ProjectId",
                table: "ProjectUsers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_UserRoots_UserId",
                table: "ProjectUsers",
                column: "UserId",
                principalTable: "UserRoots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_UserRoots_UserId",
                table: "UserConnections",
                column: "UserId",
                principalTable: "UserRoots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_Conversations_ConversationId",
                table: "UserConversations",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_UserRoots_UserId",
                table: "UserConversations",
                column: "UserId",
                principalTable: "UserRoots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoots_Accounts_AccountId",
                table: "UserRoots",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_UserRoots_UserAId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_UserRoots_UserBId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_Projects_ProjectId",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_UserRoots_UserId",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_UserRoots_UserId",
                table: "UserConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_Conversations_ConversationId",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_UserRoots_UserId",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoots_Accounts_AccountId",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "TotalExpectedInvestment",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "TotalInvestment",
                table: "ProjectUsers");

            migrationBuilder.RenameIndex(
                name: "IX_UserConversations_ConversationId",
                table: "UserConversations",
                newName: "IX_UserConversations_Conversa~");

            migrationBuilder.RenameColumn(
                name: "History",
                table: "Projects",
                newName: "GrowHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_UserRoots_U~1",
                table: "Conversations",
                column: "UserBId",
                principalTable: "UserRoots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_UserRoots_Us~",
                table: "Conversations",
                column: "UserAId",
                principalTable: "UserRoots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_Con~",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_Projects_Proj~",
                table: "ProjectUsers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_UserRoots_Use~",
                table: "ProjectUsers",
                column: "UserId",
                principalTable: "UserRoots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_UserRoots_~",
                table: "UserConnections",
                column: "UserId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoots_Accounts_Account~",
                table: "UserRoots",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
