using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class ChangeJobApplicationStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""JobApplications""
                ALTER COLUMN ""Status"" TYPE integer
                USING (
                    CASE ""Status""
                        WHEN 'Applied' THEN 0
                        WHEN 'UnderReview' THEN 1
                        WHEN 'Shortlisted' THEN 2
                        WHEN 'Hired' THEN 3
                        WHEN 'Rejected' THEN 4
                        WHEN 'Withdrawn' THEN 5
                    END
                );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""JobApplications""
                ALTER COLUMN ""Status"" TYPE text
                USING (
                    CASE ""Status""
                        WHEN 0 THEN 'Applied'
                        WHEN 1 THEN 'UnderReview'
                        WHEN 2 THEN 'Shortlisted'
                        WHEN 3 THEN 'Hired'
                        WHEN 4 THEN 'Rejected'
                        WHEN 5 THEN 'Withdrawn'
                    END
                );");
        }
    }
}
