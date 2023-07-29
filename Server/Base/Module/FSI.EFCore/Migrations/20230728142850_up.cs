using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSI.EFCore.Migrations
{
    public partial class up : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProjectWorks_ProjectId",
                table: "ProjectWorks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEvents_ProjectId",
                table: "ProjectEvents",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCalendarEvents_ProjectId",
                table: "ProjectCalendarEvents",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCalendarEvents_Projects_ProjectId",
                table: "ProjectCalendarEvents",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEvents_Projects_ProjectId",
                table: "ProjectEvents",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectWorks_Projects_ProjectId",
                table: "ProjectWorks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCalendarEvents_Projects_ProjectId",
                table: "ProjectCalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEvents_Projects_ProjectId",
                table: "ProjectEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectWorks_Projects_ProjectId",
                table: "ProjectWorks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectWorks_ProjectId",
                table: "ProjectWorks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectEvents_ProjectId",
                table: "ProjectEvents");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCalendarEvents_ProjectId",
                table: "ProjectCalendarEvents");
        }
    }
}
