using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class removeuserconnection4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStorage",
                table: "Conversations",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStorage",
                table: "Conversations");
        }
    }
}
