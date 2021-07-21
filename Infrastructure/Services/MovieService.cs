using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        private MovieDetailsResponseModel MapMovieDetail(Movie movie)
        {
            var movieDetails = new MovieDetailsResponseModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Budget = movie.Budget.GetValueOrDefault(),
                Rating = movie.Rating,
                ReleaseDate = movie.ReleaseDate,
                BackdropUrl = movie.BackdropUrl,
                ImdbUrl = movie.ImdbUrl,
                PosterUrl = movie.PosterUrl,
                Overview = movie.Overview,
                Price = movie.Price,
                RunTime = movie.RunTime,
                Revenue = movie.Revenue,
                Tagline = movie.Tagline,
            };
            movieDetails.Casts = new List<CastResponseModel>();
            foreach (var cast in movie.MovieCasts)
            {
                movieDetails.Casts.Add(new CastResponseModel
                {
                    Id = cast.CastId,
                    Name = cast.Cast.Name,
                    Character = cast.Character,
                    ProfilePath = cast.Cast.ProfilePath
                });
            }

            movieDetails.Genres = new List<GenreModel>();
            foreach (var genre in movie.Genres)
            {
                movieDetails.Genres.Add(new GenreModel()
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
            }
            return movieDetails;
        }

        private MovieCardResponseModel MapMovieCard(Movie movie)
        {
            return new MovieCardResponseModel
            {
                Id = movie.Id,
                Budget = movie.Budget.GetValueOrDefault(),
                PosterUrl = movie.PosterUrl,
                Title = movie.Title
            };
        }

        public async Task<List<MovieCardResponseModel>> GetTopRevenueMoviesAsync()
        {
            var movies = await _movieRepository.GetHighest30GrossingMoviesAsync();
            var movieCards = new List<MovieCardResponseModel>();
            foreach (var movie in movies)
            {
                movieCards.Add(new MovieCardResponseModel
                {
                    Id = movie.Id,
                    Budget = movie.Budget.GetValueOrDefault(),
                    PosterUrl = movie.PosterUrl,
                    Title = movie.Title
                });
            }

            return movieCards;
        }

        public async Task<MovieDetailsResponseModel> GetMovieDetailsAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            return MapMovieDetail(movie);
        }

        public async Task<List<MovieCardResponseModel>> GetMoviesByGenreAsync(int genre_id)
        {
            var movies = await _movieRepository.GetMoviesByGenreIdAsync(genre_id);
            var movieCards = new List<MovieCardResponseModel>();
            foreach (var movie in movies)
            {
                movieCards.Add(new MovieCardResponseModel()
                {
                    Id = movie.Id,
                    Budget = movie.Budget.GetValueOrDefault(),
                    PosterUrl = movie.PosterUrl,
                    Title = movie.Title
                });
            }
            return movieCards;
        }

        public async Task<List<MovieCardResponseModel>> GetTopRatedMoviesAsync()
        {
            var movies = await _movieRepository.GetHightset30TopRatingMoviesAsync();
            var responseModel = new List<MovieCardResponseModel>();
            foreach (var movie in movies)
            {
                responseModel.Add(MapMovieCard(movie));
            }
            return responseModel;
        }

        public async Task<List<MovieCardResponseModel>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.ListAllAsync();
            var responseModel = new List<MovieCardResponseModel>();
            foreach (var movie in movies)
            {
                responseModel.Add(MapMovieCard(movie));
            }
            return responseModel;
        }

        public async Task<List<ReviewResponseModel>> GetMovieReviewsAsync(int id)
        {
            var movie= await _movieRepository.GetMovieWithReviewsAsync(id);
            var responseModel = new List<ReviewResponseModel>();
            foreach (var m in movie.Reviews)
            {
                responseModel.Add(new ReviewResponseModel()
                {
                    MovieId = m.MovieId,
                    UserId = m.UserId,
                    MovieTitle = m.Movie.Title,
                    UserName = m.User.FirstName + " " + m.User.LastName,
                    Rating = m.Rating,
                    Review = m.ReviewText
                });
            }
            return responseModel;
        }
    }

}

