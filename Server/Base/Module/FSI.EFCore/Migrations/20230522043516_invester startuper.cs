using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class investerstartuper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BasicDescription",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "InvestFields",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "InvestorName",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "MaxInvestValue",
                table: "UserRoots",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinInvestValue",
                table: "UserRoots",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Conversations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicDescription",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "InvestFields",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "InvestorName",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "MaxInvestValue",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "MinInvestValue",
                table: "UserRoots");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Conversations");
        }
    }
}
