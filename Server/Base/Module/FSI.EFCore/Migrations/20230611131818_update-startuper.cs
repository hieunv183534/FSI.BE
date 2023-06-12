using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class updatestartuper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Award",
                table: "UserRoots");

            migrationBuilder.RenameColumn(
                name: "Certificate",
                table: "UserRoots",
                newName: "CertificateAndAward");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CertificateAndAward",
                table: "UserRoots",
                newName: "Certificate");

            migrationBuilder.AddColumn<string>(
                name: "Award",
                table: "UserRoots",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
