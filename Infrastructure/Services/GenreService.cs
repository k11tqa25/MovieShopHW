using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        private GenreModel ConvertGenreToGenreModel(Genre genre)
        {
            return new GenreModel
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public List<GenreModel> GetAllGenreModels()
        {
            var genre_repo =  _genreRepository.GetAllGenres();
            List<GenreModel> genreModels = new List<GenreModel>();
            foreach (var genre in genre_repo)
            {
                genreModels.Add(ConvertGenreToGenreModel(genre));
            }
            return genreModels;
        }

        public async Task<GenreModel> GetGenreModelByIdAsync(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            return ConvertGenreToGenreModel(genre);
        }

        public  async Task<List<GenreModel>> GetAllGenreModelsAsync()
        {
            var genre_repo = await _genreRepository.ListAllAsync();
            List<GenreModel> genreModels = new List<GenreModel>();
            foreach (var genre in genre_repo)
            {
                genreModels.Add(ConvertGenreToGenreModel(genre));
            }
            return genreModels;
        }
    }
}
