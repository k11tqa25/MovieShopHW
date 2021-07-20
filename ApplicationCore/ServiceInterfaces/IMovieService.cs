using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.ServiceInterfaces
{
    public interface IMovieService
    {
       Task<List<MovieCardResponseModel>> GetTopRevenueMoviesAsync();

        Task<MovieDetailsResponseModel>  GetMovieDetailsAsync(int id);

        Task<List<MovieCardResponseModel>> GetMoviesByGenreAsync(int genre_id);

    }
}
