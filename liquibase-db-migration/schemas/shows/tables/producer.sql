--liquibase formatted sql

--changeset robin.herd:1

CREATE TABLE shows.producer
(
    id INTEGER PRIMARY KEY NOT NULL,
    name TEXT NOT NULL
);
