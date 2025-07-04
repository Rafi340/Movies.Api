﻿using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public MovieRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();
            var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into movies (id, slug, title, yearofrelease)
                values (@Id, @Slug, @Title, @YearOfRelease)
                """, movie, cancellationToken: token));
            if(result > 0)
            {
                foreach(var genre in movie.Genres)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        insert into genres (movieId, name)
                        values (@MovieId, @Name)
                        """, new { MovieId = movie.Id , Name = genre}, cancellationToken: token));
                }
            }
            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id , CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();
            await connection.ExecuteAsync(new CommandDefinition("""
                delete from genres where movieId=@id
                """, new { id}, cancellationToken: token));
            var result = await connection.ExecuteAsync(new CommandDefinition("""
                delete from movies where id=@id
                """, new { id }, cancellationToken: token));
            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistByIdAsync(Guid id , CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                select count(1) from movies where id=@id
                """, new { id }, cancellationToken: token));
        }

        public async Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default)
        {

            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QueryAsync(new CommandDefinition("""
                select m.*, 
                
                string_agg(distinct g.name, ',') as genres,
                round(avg(r.rating),1) as rating, myr.rating as userrating 
                
                from movies m
                left join genres g on m.id = g.movieId
                left join ratings r on m.id = r.movieid
                left join ratings myr on m.id = myr.movieid
                and myr.userid = @userId
                group by m.id , myr.rating
                """, new { userId } ,cancellationToken: token));
            return result.Select(x => new Movie
            {
                Id=x.id,
                Title = x.title,
                Rating = (float?)x.rating,
                UserRating = (int?)x.userRating,
                YearOfRelease = x.yearofrelease,
                Genres = Enumerable.ToList(x.genres.Split(','))
            }
            );
        }

        public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
                new CommandDefinition("""
                    select m.*, round(avg(r.rating),1) as rating, myr.rating as userrating 
                    from movies m
                    left join ratings r on m.id = r.movieid
                    letf join ratings myr on m.id = myr.movieid
                    and myr.userid = @userId
                    where id=@id 
                    group by id, userrating
                    """, new { id , userId }, cancellationToken: token));
            if(movie is null)
            {
                return null;
            }
            var genres = await connection.QueryAsync<string>(new CommandDefinition("""
                select name from genres where movieId=@id
                """, new { id }, cancellationToken: token));
            foreach(var genre in genres)
            {
                movie.Genres.Add(genre);
            }
            return movie;
        }

        public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
                new CommandDefinition("""
                    select  m.*, round(avg(r.rating),1) as rating, myr.rating as userrating 
                    from movies m
                    left join ratings r on m.id = r.movieid
                    letf join ratings myr on m.id = myr.movieid
                    and myr.userid = @userId
                    where slug=@slug
                    group by id, userrating
                    """, new { slug, userId}, cancellationToken: token));
            if (movie is null)
            {
                return null;
            }
            var genres = await connection.QueryAsync<string>(new CommandDefinition("""
                select name from genres where movieId=@id
                """, new { id = movie.Id }, cancellationToken: token));
            foreach (var genre in genres)
            {
                movie.Genres.Add(genre);
            }
            return movie;
        }

        public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();
            await connection.ExecuteAsync(new CommandDefinition("""
                delete from genres where movieId=@id
                """, new { id = movie.Id }, cancellationToken: token));
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                        insert into genres (movieId, name)
                        values (@MovieId, @Name)
                        """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
            }
            var result = await connection.ExecuteAsync(new CommandDefinition("""
                Update movies set id=@Id, slug=@Slug, title=@Title, yearofrelease=@YearOfRelease where id=@Id
                """,movie, cancellationToken: token));
            transaction.Commit();
            return result > 0;
        }
    }
}
