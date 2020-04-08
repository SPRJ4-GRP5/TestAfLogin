using Microsoft.EntityFrameworkCore.Migrations;

namespace TestAfLogin.Migrations
{
    public partial class pictureinregister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameOfUser",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imageName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameOfUser",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "imageName",
                table: "AspNetUsers");
        }
    }
}
