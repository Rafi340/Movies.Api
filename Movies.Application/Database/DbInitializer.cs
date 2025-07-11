﻿using System.Data;
using Dapper;

namespace Movies.Application.Database
{
    public class DbInitializer
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public DbInitializer(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task InitializeAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync("""
                create table if not exists movies (
                    id UUID primary key,
                    title TEXT not null,
                    slug TEXT not null,
                    yearofrelease integer not null
                );
                """);
            await connection.ExecuteAsync("""
                create unique index if not exists movies_slug_idx 
                on movies
                using btree (slug)
                """);
            await connection.ExecuteAsync("""
                create table if not exists genres (
                movieId UUID references movies (id),
                name TEXT not null);
                """);
            await connection.ExecuteAsync("""
                create table if not exists ratings (
                    userid UUID,
                    movieid UUID references movies (id),
                    rating integer not null,
                    primary key (userid,movieid)
                );
                """);
        }
    }
}
