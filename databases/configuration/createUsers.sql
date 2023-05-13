DO
$do$
BEGIN
   IF EXISTS (
      SELECT FROM pg_catalog.pg_roles
      WHERE  rolname = 'configurationdb_rw') THEN

      RAISE NOTICE 'Role "configurationdb_rw" already exists. Skipping.';
   ELSE
      CREATE ROLE configurationdb_rw LOGIN;
   END IF;
END
$do$;