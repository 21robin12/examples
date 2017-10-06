# node-postgres-getting-started

Getting started with PostgreSQL, and running queries from NodeJS

### Postgres setup

 - Install PostgreSQL: https://www.enterprisedb.com/downloads/postgres-postgresql-downloads#windows 
 - When creating a user as part of the installation, take note of the username & password entered
 - Install pgAdmin: https://www.postgresql.org/ftp/pgadmin/pgadmin4/v1.6/windows/ 
 - Open pgAdmin; connect to local server at "localhost" using username and password from installation process
 - The tree view on the left in pgAdmin shows a load of different nodes - these can be hidden at File / Preferences / Browser / Nodes
 - Create a database on the local server, e.g. "test"
 - Run the following scripts to create a schema with a single table
 
```
create schema booking;

create table booking.day (
    id int not null,
    name varchar(10) not null
);

insert into booking.day (id, name) values (1, 'Monday');
insert into booking.day (id, name) values (2, 'Tuesday');
insert into booking.day (id, name) values (3, 'Wednesday');
insert into booking.day (id, name) values (4, 'Thursday');
insert into booking.day (id, name) values (5, 'Friday');
insert into booking.day (id, name) values (6, 'Saturday');
insert into booking.day (id, name) values (7, 'Sunday');

alter table booking.day add column bookedBy varchar(20) null;

update booking.day set bookedby = 'joe.bloggs' where id = 3;

select * from booking.day
order by id;
```

### Calling from Node

 - Install Node: https://nodejs.org/en/download/
 - Edit `server.js` connection string with username, password and database name for local postgres server
 - Open command prompt and navigate to this directory
 - Run `npm intall` 
 - Run `node server` 
