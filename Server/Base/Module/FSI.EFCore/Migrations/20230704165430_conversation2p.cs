using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class conversation2p : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Conversations",
                newName: "IsActiveB");

            migrationBuilder.AddColumn<bool>(
                name: "IsActiveA",
                table: "Conversations",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveA",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "IsActiveB",
                table: "Conversations",
                newName: "IsActive");
        }
    }
}
