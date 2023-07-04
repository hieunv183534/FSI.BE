using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class removeuserconnection5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsStorage",
                table: "Conversations",
                newName: "IsStorageB");

            migrationBuilder.AddColumn<bool>(
                name: "IsStorageA",
                table: "Conversations",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStorageA",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "IsStorageB",
                table: "Conversations",
                newName: "IsStorage");
        }
    }
}
