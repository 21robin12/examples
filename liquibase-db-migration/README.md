# Liquibase

Liquibase is a database migration tool. This repo is a demo of using Liquibase with a Postgres DB.

# Drivers

When installing a driver through DataGrip, the files are placed in:

C:\Users\robin.herd\.DataGrip2016.3\config\jdbc-drivers

(can use these for liquibase - see migrate.bat "classpath" arg)

# Running

 - Ensure a database is running locally with the values from migrate.bat (DB name, username, password etc.)
 - Ensure classpath points to the jar file of the postgres driver (can be same path as DataGrip, or could download drivers separately)
 - Run "migrate" from the command line
 - `databasechangelog` table in DB should be created / updated, and all the relevant migration scripts should be applied

# Useful links

 - https://blog.codecentric.de/en/2015/01/managing-database-migrations-using-liquibase/
 - http://www.liquibase.org/quickstart.html
 - http://www.liquibase.org/documentation/sql_format.html