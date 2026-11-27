using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class UniqueEmployeeIdOnCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Companies_EmployeeId",
                table: "Companies",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_EmployeeId",
                table: "Companies");
        }
    }
}
