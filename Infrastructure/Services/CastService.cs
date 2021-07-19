using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CastService : ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastService(ICastRepository castRepository)
        {
            _castRepository = castRepository;
        }
        public async Task<CastResponseModel> GetCastDetailsAsync(int cast_id, int movie_id)
        {
            var cast = await _castRepository.GetCastByIdWithAllInfoAsync(cast_id);
            string character = cast.MovieCasts.Where(mc => mc.MovieId == movie_id).FirstOrDefault().Character;
            return new CastResponseModel
            {
                Id = cast.Id,
                Name = cast.Name,
                Gender = cast.Gender,
                ProfilePath = cast.ProfilePath,
                TmdbUrl = cast.TmdbUrl,
                Character = character
            };
        }
    }
}
