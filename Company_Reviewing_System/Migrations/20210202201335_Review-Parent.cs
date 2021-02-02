using Microsoft.EntityFrameworkCore.Migrations;

namespace Company_Reviewing_System.Migrations
{
    public partial class ReviewParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyPageCompanyId",
                table: "Review",
                newName: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Review",
                newName: "CompanyPageCompanyId");
        }
    }
}
