---
layout: default
title: Development
parent: Development Environment
nav_order: 1
has_children: false
---

docker run --restart always -p 8080:80 -e 'PGADMIN_DEFAULT_EMAIL=user@domain.com' -e 'PGADMIN_DEFAULT_PASSWORD=SuperSecret' -d dpage/pgadmin4


& 'C:\Program Files\Docker\docker-compose_2.7.0.exe' -f .\docker-compose-backends.yml up -d


https://www.postgresql.org/docs/current/postgres-fdw.html

0)
CREATE EXTENSION IF NOT EXISTS postgres_fdw
    SCHEMA public
    VERSION "1.1";

1) https://www.postgresql.org/docs/current/sql-createserver.html
CREATE SERVER IF NOT EXISTS myserver FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'configurationDB', port '5432');

2) https://www.postgresql.org/docs/current/sql-createusermapping.html
CREATE USER MAPPING IF NOT EXISTS FOR bloguser SERVER myserver OPTIONS (user 'bloguser', password 'bloguser');

3) https://www.postgresql.org/docs/current/sql-importforeignschema.html
IMPORT FOREIGN SCHEMA public
    FROM SERVER myserver INTO public;

4) https://www.postgresql.org/docs/current/rules-materializedviews.html
CREATE MATERIALIZED VIEW sales_summary AS
Select
	"Id",
	"Name"
From public."Credential";

CREATE UNIQUE INDEX sales_summary_seller
  ON sales_summary ("Id");

