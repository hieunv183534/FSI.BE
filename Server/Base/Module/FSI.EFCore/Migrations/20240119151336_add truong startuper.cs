using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class addtruongstartuper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Purpose",
                table: "UserRoots",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialize",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ideaField",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "targetField",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "targetSpecialize",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "Specialize",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "ideaField",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "targetField",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "targetSpecialize",
                table: "UserRoots");
        }
    }
}
