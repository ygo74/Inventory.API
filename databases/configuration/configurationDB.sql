CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE SEQUENCE credential_ids START WITH 1 INCREMENT BY 10 NO MINVALUE NO MAXVALUE NO CYCLE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE SEQUENCE datacenter_ids START WITH 1 INCREMENT BY 10 NO MINVALUE NO MAXVALUE NO CYCLE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE SEQUENCE plugin_ids START WITH 1 INCREMENT BY 10 NO MINVALUE NO MAXVALUE NO CYCLE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE TABLE "Datacenter" (
        "Id" integer NOT NULL,
        "Code" text NULL,
        "Name" text NULL,
        "DataCenterType" integer NOT NULL,
        "Created" timestamp without time zone NOT NULL,
        "CreatedBy" text NULL,
        "LastModified" timestamp without time zone NULL,
        "LastModifiedBy" text NULL,
        "Deprecated" boolean NOT NULL,
        "StartDate" timestamp without time zone NOT NULL,
        "EndDate" timestamp without time zone NULL,
        "InventoryCode" text NULL,
        CONSTRAINT "PK_Datacenter" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE TABLE "Plugin" (
        "Id" integer NOT NULL,
        "Name" text NULL,
        "Code" text NULL,
        "Created" timestamp without time zone NOT NULL,
        "CreatedBy" text NULL,
        "LastModified" timestamp without time zone NULL,
        "LastModifiedBy" text NULL,
        "Deprecated" boolean NOT NULL,
        "StartDate" timestamp without time zone NOT NULL,
        "EndDate" timestamp without time zone NULL,
        "InventoryCode" text NULL,
        CONSTRAINT "PK_Plugin" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE TABLE "Credential" (
        "Id" integer NOT NULL,
        "Name" text NULL,
        "Description" text NULL,
        "PluginProviderId" integer NULL,
        "Created" timestamp without time zone NOT NULL,
        "CreatedBy" text NULL,
        "LastModified" timestamp without time zone NULL,
        "LastModifiedBy" text NULL,
        CONSTRAINT "PK_Credential" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Credential_Plugin_PluginProviderId" FOREIGN KEY ("PluginProviderId") REFERENCES "Plugin" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE UNIQUE INDEX "IX_Credential_Name" ON "Credential" ("Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE INDEX "IX_Credential_PluginProviderId" ON "Credential" ("PluginProviderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE UNIQUE INDEX "IX_Datacenter_Code" ON "Datacenter" ("Code");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    CREATE UNIQUE INDEX "IX_Plugin_Name" ON "Plugin" ("Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106205710_Initial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20221106205710_Initial', '6.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106210008_PluginPath') THEN
    ALTER TABLE "Plugin" ADD "Path" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221106210008_PluginPath') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20221106210008_PluginPath', '6.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Plugin" ALTER COLUMN "StartDate" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Plugin" ALTER COLUMN "LastModified" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Plugin" ALTER COLUMN "EndDate" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Plugin" ALTER COLUMN "Created" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Datacenter" ALTER COLUMN "StartDate" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Datacenter" ALTER COLUMN "LastModified" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Datacenter" ALTER COLUMN "EndDate" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Datacenter" ALTER COLUMN "Created" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Credential" ALTER COLUMN "LastModified" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    ALTER TABLE "Credential" ALTER COLUMN "Created" TYPE timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221128100324_FixDateColumns') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20221128100324_FixDateColumns', '6.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    ALTER TABLE "Credential" DROP CONSTRAINT "FK_Credential_Plugin_PluginProviderId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    DROP INDEX "IX_Credential_PluginProviderId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    ALTER TABLE "Credential" DROP COLUMN "PluginProviderId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    CREATE SEQUENCE location_ids START WITH 1 INCREMENT BY 10 NO MINVALUE NO MAXVALUE NO CYCLE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    ALTER TABLE "Plugin" ADD "Version" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    ALTER TABLE "Datacenter" ADD "Description" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    ALTER TABLE "Datacenter" ADD "LocationId" integer NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    ALTER TABLE "Credential" ADD "PropertyBag" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    CREATE TABLE "Location" (
        "Id" integer NOT NULL,
        "Name" text NULL,
        "Description" text NULL,
        "CountryCode" text NULL,
        "CityCode" text NULL,
        "RegionCode" text NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" text NULL,
        "LastModified" timestamp with time zone NULL,
        "LastModifiedBy" text NULL,
        "Deprecated" boolean NOT NULL,
        "StartDate" timestamp with time zone NOT NULL,
        "EndDate" timestamp with time zone NULL,
        "InventoryCode" text NULL,
        CONSTRAINT "PK_Location" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    CREATE TABLE "PluginEndpoint" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "DatacenterId" integer NOT NULL,
        "CredentialId" integer NULL,
        "PluginId" integer NULL,
        "PropertyBag" text NULL,
        "Created" timestamp with time zone NOT NULL,
        "CreatedBy" text NULL,
        "LastModified" timestamp with time zone NULL,
        "LastModifiedBy" text NULL,
        CONSTRAINT "PK_PluginEndpoint" PRIMARY KEY ("DatacenterId", "Id"),
        CONSTRAINT "FK_PluginEndpoint_Credential_CredentialId" FOREIGN KEY ("CredentialId") REFERENCES "Credential" ("Id"),
        CONSTRAINT "FK_PluginEndpoint_Datacenter_DatacenterId" FOREIGN KEY ("DatacenterId") REFERENCES "Datacenter" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_PluginEndpoint_Plugin_PluginId" FOREIGN KEY ("PluginId") REFERENCES "Plugin" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    CREATE INDEX "IX_Datacenter_LocationId" ON "Datacenter" ("LocationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    CREATE UNIQUE INDEX "IX_Location_CityCode_CountryCode_RegionCode" ON "Location" ("CityCode", "CountryCode", "RegionCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    CREATE INDEX "IX_PluginEndpoint_CredentialId" ON "PluginEndpoint" ("CredentialId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    CREATE INDEX "IX_PluginEndpoint_PluginId" ON "PluginEndpoint" ("PluginId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    ALTER TABLE "Datacenter" ADD CONSTRAINT "FK_Datacenter_Location_LocationId" FOREIGN KEY ("LocationId") REFERENCES "Location" ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20221201172323_newEntities') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20221201172323_newEntities', '6.0.11');
    END IF;
END $EF$;
COMMIT;

