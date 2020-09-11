using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCore.Migrations
{
    public partial class updateattraa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tb_m_division_DepartmentId",
                table: "tb_m_division",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_division_tb_m_department_DepartmentId",
                table: "tb_m_division",
                column: "DepartmentId",
                principalTable: "tb_m_department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_division_tb_m_department_DepartmentId",
                table: "tb_m_division");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_division_DepartmentId",
                table: "tb_m_division");
        }
    }
}
