--liquibase formatted sql

--changeset robin.herd:1

CREATE TABLE shows.show
(
    id INTEGER PRIMARY KEY NOT NULL,
    name TEXT NOT NULL
);

--changeset robin.herd:2

ALTER TABLE shows.show ADD COLUMN channel VARCHAR(30);

--changeset robin.herd:3

ALTER TABLE shows.show ADD COLUMN producerId INTEGER REFERENCES shows.producer(id);