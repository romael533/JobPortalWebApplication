using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class RenameEmployeeIdToEmployerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Jobs",
                newName: "EmployerId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Companies",
                newName: "EmployerId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_EmployeeId",
                table: "Companies",
                newName: "IX_Companies_EmployerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployerId",
                table: "Jobs",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "EmployerId",
                table: "Companies",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_EmployerId",
                table: "Companies",
                newName: "IX_Companies_EmployeeId");
        }
    }
}
