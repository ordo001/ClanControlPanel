CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "ClanMoney" (
    "Id" uuid NOT NULL,
    "ActionDate" timestamp with time zone NOT NULL,
    "TotalAmountAfterAction" numeric NOT NULL,
    "ChangeAmount" numeric NOT NULL,
    "Reason" integer NOT NULL,
    "CustomReason" text,
    CONSTRAINT "PK_ClanMoney" PRIMARY KEY ("Id")
);

CREATE TABLE "EventTypes" (
    "Id" uuid NOT NULL,
    "NameEventType" text NOT NULL,
    CONSTRAINT "PK_EventTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "Items" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    CONSTRAINT "PK_Items" PRIMARY KEY ("Id")
);

CREATE TABLE "Squads" (
    "Id" uuid NOT NULL,
    "NameSquad" text NOT NULL,
    CONSTRAINT "PK_Squads" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Login" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Name" text NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Event" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "EventTypeId" uuid NOT NULL,
    "Status" integer,
    CONSTRAINT "PK_Event" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Event_EventTypes_EventTypeId" FOREIGN KEY ("EventTypeId") REFERENCES "EventTypes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Players" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "UserId" uuid NOT NULL,
    "SquadId" uuid,
    CONSTRAINT "PK_Players" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Players_Squads_SquadId" FOREIGN KEY ("SquadId") REFERENCES "Squads" ("Id"),
    CONSTRAINT "FK_Players_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Equipments" (
    "Id" uuid NOT NULL,
    "PlayerId" uuid NOT NULL,
    "ItemId" uuid NOT NULL,
    CONSTRAINT "PK_Equipments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Equipments_Items_ItemId" FOREIGN KEY ("ItemId") REFERENCES "Items" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Equipments_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "EventAttendences" (
    "Id" uuid NOT NULL,
    "EventId" uuid NOT NULL,
    "PlayerId" uuid NOT NULL,
    "WasPresent" boolean NOT NULL,
    "IsExcused" boolean,
    "AbsenceReason" text,
    CONSTRAINT "PK_EventAttendences" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_EventAttendences_Event_EventId" FOREIGN KEY ("EventId") REFERENCES "Event" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EventAttendences_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Schedule" (
    "Id" uuid NOT NULL,
    "PlayerId" uuid NOT NULL,
    "DayOfWeek" integer NOT NULL,
    CONSTRAINT "PK_Schedule" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Schedule_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Equipments_ItemId" ON "Equipments" ("ItemId");

CREATE INDEX "IX_Equipments_PlayerId" ON "Equipments" ("PlayerId");

CREATE INDEX "IX_Event_EventTypeId" ON "Event" ("EventTypeId");

CREATE INDEX "IX_EventAttendences_EventId" ON "EventAttendences" ("EventId");

CREATE INDEX "IX_EventAttendences_PlayerId" ON "EventAttendences" ("PlayerId");

CREATE INDEX "IX_Players_SquadId" ON "Players" ("SquadId");

CREATE UNIQUE INDEX "IX_Players_UserId" ON "Players" ("UserId");

CREATE INDEX "IX_Schedule_PlayerId" ON "Schedule" ("PlayerId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250508132534_Initial', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "Users" ADD "Role" text NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250508142735_migration1', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "Event" DROP COLUMN "Name";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250508171150_migration2', '8.0.0');

COMMIT;

START TRANSACTION;

DROP TABLE "EventAttendences";

DROP TABLE "Event";

ALTER TABLE "ClanMoney" DROP COLUMN "CustomReason";

ALTER TABLE "ClanMoney" ALTER COLUMN "Reason" TYPE text;
ALTER TABLE "ClanMoney" ALTER COLUMN "Reason" DROP NOT NULL;

CREATE TABLE "Events" (
    "Id" uuid NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "EventTypeId" uuid NOT NULL,
    "Status" integer,
    CONSTRAINT "PK_Events" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Events_EventTypes_EventTypeId" FOREIGN KEY ("EventTypeId") REFERENCES "EventTypes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "EventAttendances" (
    "Id" uuid NOT NULL,
    "EventId" uuid NOT NULL,
    "PlayerId" uuid NOT NULL,
    "WasPresent" boolean NOT NULL,
    "Status" integer NOT NULL,
    "AbsenceReason" text,
    CONSTRAINT "PK_EventAttendances" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_EventAttendances_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EventAttendances_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "EventStages" (
    "Id" uuid NOT NULL,
    "EventId" uuid NOT NULL,
    "StageNumber" integer NOT NULL,
    "Description" text,
    "Amount" integer NOT NULL,
    CONSTRAINT "PK_EventStages" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_EventStages_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_EventAttendances_EventId" ON "EventAttendances" ("EventId");

CREATE INDEX "IX_EventAttendances_PlayerId" ON "EventAttendances" ("PlayerId");

CREATE INDEX "IX_Events_EventTypeId" ON "Events" ("EventTypeId");

CREATE INDEX "IX_EventStages_EventId" ON "EventStages" ("EventId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250520094356_migration3', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "EventAttendances" DROP COLUMN "WasPresent";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250520095029_migration4', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "Players" ADD "Position" integer;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250525105846_migration5', '8.0.0');

COMMIT;

