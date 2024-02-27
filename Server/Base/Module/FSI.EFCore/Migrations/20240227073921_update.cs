using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hasIdea",
                table: "UserRoots");

            migrationBuilder.RenameColumn(
                name: "targetSpecialize",
                table: "UserRoots",
                newName: "TargetSpecialize");

            migrationBuilder.RenameColumn(
                name: "targetField",
                table: "UserRoots",
                newName: "TargetField");

            migrationBuilder.RenameColumn(
                name: "ideaField",
                table: "UserRoots",
                newName: "IdeaField");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetSpecialize",
                table: "UserRoots",
                newName: "targetSpecialize");

            migrationBuilder.RenameColumn(
                name: "TargetField",
                table: "UserRoots",
                newName: "targetField");

            migrationBuilder.RenameColumn(
                name: "IdeaField",
                table: "UserRoots",
                newName: "ideaField");

            migrationBuilder.AddColumn<bool>(
                name: "hasIdea",
                table: "UserRoots",
                type: "tinyint(1)",
                nullable: true);
        }
    }
}
