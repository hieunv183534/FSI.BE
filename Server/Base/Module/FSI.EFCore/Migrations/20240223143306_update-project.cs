using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class updateproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableTimeRequire",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsHireNewMember",
                table: "Projects");

            migrationBuilder.AddColumn<bool>(
                name: "IsProfit",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Scale",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkingForm",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProfit",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Scale",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WorkingForm",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "AvailableTimeRequire",
                table: "Projects",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsHireNewMember",
                table: "Projects",
                type: "tinyint(1)",
                nullable: true);
        }
    }
}
