using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class updatehiring1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Income",
                table: "ProjectHiring");

            migrationBuilder.AlterColumn<int>(
                name: "Scale",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ProjectHiring",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "ProjectHiring",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ProjectHiring",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ProjectHiring",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncodeMode",
                table: "ProjectHiring",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncomeFrom",
                table: "ProjectHiring",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeRange",
                table: "ProjectHiring",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeTo",
                table: "ProjectHiring",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectHiring",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ProjectHiring",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "ProjectHiring",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "IncodeMode",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "IncomeFrom",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "IncomeRange",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "IncomeTo",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ProjectHiring");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "ProjectHiring");

            migrationBuilder.AlterColumn<int>(
                name: "Scale",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Income",
                table: "ProjectHiring",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
