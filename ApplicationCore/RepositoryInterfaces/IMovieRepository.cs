using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace ApplicationCore.RepositoryInterfaces
{
    public interface IMovieRepository: IAsyncRepository<Movie>
    {
       Task<List<Movie>> GetHighest30GrossingMoviesAsync();

        Task<List<Movie>> GetMoviesByGenreIdAsync(int genre_id);
        
    }
}
