using Microsoft.EntityFrameworkCore.Migrations;

namespace TestAfLogin.Migrations
{
    public partial class newFieldOfStudy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_FieldOfStudyModel_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FieldOfStudyModel_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyModel_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FieldOfStudy",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldOfStudy",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FieldOfStudyModel_ApplicationUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FieldOfStudyModel_ApplicationUserId",
                table: "AspNetUsers",
                column: "FieldOfStudyModel_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_FieldOfStudyModel_ApplicationUserId",
                table: "AspNetUsers",
                column: "FieldOfStudyModel_ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
