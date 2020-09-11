using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCore.Migrations
{
    public partial class addEmployeeAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Employee",
                newName: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Employee",
                newName: "Adress");
        }
    }
}
