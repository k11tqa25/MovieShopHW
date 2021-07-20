using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovieRepository : EFRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Movie>> GetHighest30GrossingMoviesAsync()
        {
            var topMovies = await _dbContext.Movies.OrderByDescending(m=>m.Revenue).Take(30).ToListAsync();
            return topMovies;            
        }

        public override async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movies.Include(m => m.MovieCasts)
                                                                                   .ThenInclude(m => m.Cast)
                                                                                   .Include(m => m.Genres)
                                                                                   .FirstOrDefaultAsync(m => m.Id == id);
            if(movie == null)
            {
                throw new Exception($"No movie found with id = {id}");
            }

            var movieRating = await _dbContext.Reviews.Where(r => r.MovieId == id)
                                                                                                 .DefaultIfEmpty()
                                                                                                 .AverageAsync(r => r == null ? 0 : r.Rating);

            if (movieRating > 0) movie.Rating = movieRating;

            return movie;
        }

        public async Task<List<Movie>> GetMoviesByGenreIdAsync(int genre_id)
        {
            var genre = await _dbContext.Genres.Include(g => g.Movies).Where(g => g.Id == genre_id).FirstOrDefaultAsync();

            return genre.Movies.ToList();
        }
    }
}
