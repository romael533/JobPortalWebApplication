START TRANSACTION;
CREATE TABLE "Candidates" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "Phone" text NOT NULL,
    "Address" text,
    "Skills" text NOT NULL,
    "ExperienceSummary" text NOT NULL,
    "LanguagesKnown" text,
    "CandidateId" text NOT NULL,
    CONSTRAINT "PK_Candidates" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Candidates_CandidateId" ON "Candidates" ("CandidateId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261127173123_AddCandidates', '10.0.0');

COMMIT;

