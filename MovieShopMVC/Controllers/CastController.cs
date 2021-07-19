using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MovieShopMVC.Controllers
{
    public class CastController : BaseController
    {
        private readonly ICastService _castService ; 
        public CastController( ICastService castService, IGenreService genreService): base(genreService) 
        {
            _castService = castService;
        }

        public async Task<IActionResult> Details(int cast_id, int movie_id)
        {
            var castModel = await _castService.GetCastDetailsAsync(cast_id, movie_id);
            return View(castModel);
        }
    }
}
