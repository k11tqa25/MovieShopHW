using ApplicationCore.Models;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShopMVC.Controllers
{
    public class BaseController: Controller
    {
        public List<GenreModel> Genres { get; private set; }

        public BaseController(IGenreService genreService)
        {
            Genres = genreService.GetAllGenreModels();
        }

        public override ViewResult View()
        {
            ViewData["Genres"] = Genres;
            return base.View();
        }
        public override ViewResult View(object model)
        {
            ViewData["Genres"] = Genres;
            return base.View(model);
        }
        public override ViewResult View(string viewName)
        {
            ViewData["Genres"] = Genres;
            return base.View(viewName);
        }
        public override ViewResult View(string viewName, object model)
        {
            ViewData["Genres"] = Genres;
            return base.View(viewName, model);
        }
    }
}
