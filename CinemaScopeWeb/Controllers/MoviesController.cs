using System.Web.Mvc;
using CinemaScopeWeb.ViewModels;
using Microsoft.AspNet.Identity;
using MovieService.Interfaces;
using MovieService.Interfaces.ServiceInterfaces;

namespace CinemaScopeWeb.Controllers
{
    public class MoviesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        IImdbService _imdbService;
        IMovieService _moviesService;

        public MoviesController(IUnitOfWork unitOfWork, IImdbService imdbService, IMovieService moviesService)
        {
            _unitOfWork = unitOfWork;
            _imdbService = imdbService;
            _moviesService = moviesService;
        }

        public ActionResult Get(int id)
        {
            var model = new MovieViewModel() { Movie = _unitOfWork.MovieRepository.GetById(id) };

            if (model.Movie == null) return View("NoMovie");

            var userId = User.Identity.GetUserId();
            var userToMovie = _unitOfWork.UserToMovieRepository.GetOneByUserAndMovieIds(userId, model.Movie.Id);
            model.UserRating = _moviesService.GetUserRating(model.Movie.Id);
            if (User.Identity.IsAuthenticated && !(userToMovie is null))
            {
                model.IsLiked = userToMovie.IsLiked;
                model.IsWatched = userToMovie.IsWatched;
                model.IsDisliked = userToMovie.IsDisLiked;
            }
            return View(model);
        }

        [Authorize]
        public ActionResult LikeMovie(int id)
        {
            var userId = User.Identity.GetUserId();
            _moviesService.LikeMovie(userId, id);
            return RedirectToAction("Get", new { id });
        }

        [Authorize]
        public ActionResult DislikeMovie(int id)
        {
            var userId = User.Identity.GetUserId();
            _moviesService.DislikeMovie(userId, id);
            return RedirectToAction("Get", new {id});
        }

        [Authorize]
        public ActionResult MarkAsWatched(int id)
        {
            var userId = User.Identity.GetUserId();
            _moviesService.MarkAsWatched(userId, id);
            return RedirectToAction("Get", new { id });
        }

        public ActionResult Top250()
        {
            var data = _imdbService.GetTop250();
            return !string.IsNullOrEmpty(data.ErrorMessage) ? View("Error") : View(data.Items);
        }

        public ActionResult MostWatched()
        {
            return View(_moviesService.MostWatched());
        }

        public ActionResult MostLiked()
        {
            return View(_moviesService.MostLiked());
        }

        public ActionResult RatingsNavigation()
        {
            return View();
        }
    }
}