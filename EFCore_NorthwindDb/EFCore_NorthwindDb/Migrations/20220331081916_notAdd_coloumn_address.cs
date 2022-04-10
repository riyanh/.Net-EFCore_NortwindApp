using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_NortwindDb.Migrations
{
    public partial class notAdd_coloumn_address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Students");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
