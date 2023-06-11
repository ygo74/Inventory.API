DO
$do$
BEGIN
   IF EXISTS (
      SELECT FROM pg_catalog.pg_roles
      WHERE  rolname = 'devicesdb_rw') THEN

      RAISE NOTICE 'Role "devicesdb_rw" already exists. Skipping.';
   ELSE
      CREATE ROLE devicesdb_rw LOGIN;
   END IF;
END
$do$;