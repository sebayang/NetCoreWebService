using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCore.Migrations
{
    public partial class addfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_division_tb_m_department_departmentId",
                table: "tb_m_division");

            migrationBuilder.AlterColumn<int>(
                name: "departmentId",
                table: "tb_m_division",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_division_tb_m_department_departmentId",
                table: "tb_m_division",
                column: "departmentId",
                principalTable: "tb_m_department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_division_tb_m_department_departmentId",
                table: "tb_m_division");

            migrationBuilder.AlterColumn<int>(
                name: "departmentId",
                table: "tb_m_division",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_division_tb_m_department_departmentId",
                table: "tb_m_division",
                column: "departmentId",
                principalTable: "tb_m_department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
