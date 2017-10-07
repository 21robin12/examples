--liquibase formatted sql

--changeset robin.herd:1

CREATE TABLE shows.episode
(
    id INTEGER PRIMARY KEY NOT NULL,
    showId INTEGER REFERENCES shows.show(id),
    name TEXT NOT NULL
);