START TRANSACTION;

                ALTER TABLE "JobApplications"
                ALTER COLUMN "Status" TYPE text
                USING (
                    CASE "Status"
                        WHEN 0 THEN 'Applied'
                        WHEN 1 THEN 'UnderReview'
                        WHEN 2 THEN 'Shortlisted'
                        WHEN 3 THEN 'Hired'
                        WHEN 4 THEN 'Rejected'
                        WHEN 5 THEN 'Withdrawn'
                    END
                );

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261128091623_ChangeJobApplicationStatusToString', '10.0.0');

COMMIT;

