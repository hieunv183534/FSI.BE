using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class updaetstartuper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableTime",
                table: "UserRoots",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Describe",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "YearOfExp",
                table: "UserRoots",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableTime",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "Describe",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "YearOfExp",
                table: "UserRoots");
        }
    }
}
