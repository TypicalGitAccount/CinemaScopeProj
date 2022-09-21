using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CinemaScopeWeb.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using MovieService.Imdb;
using MovieService.Interfaces;
using MovieService.Interfaces.ServiceInterfaces;
using MovieService.Interfaces.ServicesInterfaces;
using PagedList;

namespace CinemaScopeWeb.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IFilteringService _filteringService;
        private IImdbService _imdbService;

        public HomeController(IUnitOfWork unitOfWork, IFilteringService filteringService, IImdbService imdbService)
        {
            _unitOfWork = unitOfWork;
            _filteringService = filteringService;
            _imdbService = imdbService;
        }

        public ActionResult Index(int page = 1)
        {
            var movies = _unitOfWork.MovieRepository.GetAllNewestFirst()
                .Select(movie => new MovieToHomeViewModel()
                {
                    Id = movie.Id,
                    Poster = movie.Poster,
                    Title = movie.Title
                }).ToPagedList(page, 10);

            var model = new HomeViewModel()
            {
                Movies = movies,
                Genres = _unitOfWork.GenreRepository.GetAll().Select(x => x.Name).Distinct().OrderBy(x => x).ToList(),
                Countries = _unitOfWork.CountryRepository.GetAll().Select(x => x.Name).Distinct().OrderBy(x => x).ToList(),
                Types = _unitOfWork.MovieTypeRepository.GetAll().Select(x => x.Name).Distinct().OrderBy(x => x).ToList(),
                Years = _unitOfWork.MovieRepository.GetAll().Select(x => int.Parse(x.Year)).Distinct().OrderBy(x => x).ToList(),
                IsWatched = false
            };
            return View(model);
        }
        
        [HttpPost]
        public ActionResult SearchResult(string input)
        {
            if (input.IsNullOrWhiteSpace())
                return RedirectToAction("Index");
            var moviesToView = _unitOfWork.MovieRepository.GetAll()
                .Select(movie => new MovieToHomeViewModel()
                {
                    Id = movie.Id,
                    Poster = movie.Poster,
                    Title = movie.Title
                }).ToList();
            var inputRegex = new Regex($"(\\b{input.ToUpper()})|(\\b{input.ToUpper()}\\b)");
            var movieWithFiltering = moviesToView
                .Where(word => inputRegex.IsMatch(word.Title.ToUpper())).ToList();
            var model = new FilteringViewModel()
            {
                Movies = movieWithFiltering,
                Genres = _unitOfWork.GenreRepository.GetAll().Select(x=>x.Name).Distinct().OrderBy(x => x).ToList(),
                Countries = _unitOfWork.CountryRepository.GetAll().Select(x=>x.Name).Distinct().OrderBy(x => x).ToList(),
                Types = _unitOfWork.MovieTypeRepository.GetAll().Select(x=>x.Name).Distinct().OrderBy(x => x).ToList(),
                Years = _unitOfWork.MovieRepository.GetAll().Select(x=>int.Parse(x.Year)).Distinct().OrderBy(x=>x).ToList(),
                IsWatched = false
            };

            return View("FilteringResult", model);
        }
        
        [HttpPost]
        public ActionResult FilteringResult(List<string> genres, 
            List<string> countries, List<string> types, List<string> years, bool isWatched = false, int page = 1)
        {
            var movies = _unitOfWork.MovieRepository.GetAll().ToList();
            _filteringService.FilterByCountries(countries,movies);
            _filteringService.FilterByGenres(genres,movies);
            _filteringService.FilterByYears(years,movies);
            _filteringService.FilterByType(types,movies);
            if (User.Identity.IsAuthenticated)
                _filteringService.FilterByWatched(isWatched, movies, User.Identity.GetUserId());
            
            var moviesWithFiltering = movies
                .Select(movie => new MovieToHomeViewModel()
                {
                    Id = movie.Id,
                    Poster = movie.Poster,
                    Title = movie.Title
                }).ToList();

            var model = new FilteringViewModel()
            {
                Movies = moviesWithFiltering,
                Genres = _unitOfWork.GenreRepository.GetAll().Select(x => x.Name).Distinct().OrderBy(x => x).ToList(),
                Countries = _unitOfWork.CountryRepository.GetAll().Select(x => x.Name).Distinct().OrderBy(x => x).ToList(),
                Types = _unitOfWork.MovieTypeRepository.GetAll().Select(x => x.Name).Distinct().OrderBy(x => x).ToList(),
                Years = _unitOfWork.MovieRepository.GetAll().Select(x => int.Parse(x.Year)).Distinct().OrderBy(x => x).ToList(),
                IsWatched = false
            };
            return View("FilteringResult", model);
        }

        public void Update()
        {
            string newMovieId;
            var lastLoadedMovie = _unitOfWork.MovieRepository.GetLastUploadedFromImdb();
            if (lastLoadedMovie != null)
            {
                newMovieId = lastLoadedMovie.ImdbId;
            }
            else
            {
                newMovieId = ImdbApi.MoiveIdCode + ImdbApi.MovieIdStartNumber;
            }
            for (int i = 0; i < ImdbApi.MaxMovieUpdates; i++)
            {
                newMovieId = IncrementId(newMovieId);
                var movieAdded = AddNewMovie(newMovieId);
                int tries = 0;
                while (!movieAdded && tries < ImdbApi.MaxMovieUpdateTries)
                {
                    newMovieId = IncrementId(newMovieId);
                    movieAdded = AddNewMovie(newMovieId);
                    tries++;
                }
            }

        }

        private string IncrementId(string id)
        {
            var lastLoadedId = id;
            var lastLoadedIdNumber = int.Parse(lastLoadedId.Replace(ImdbApi.MoiveIdCode, ""));
            lastLoadedIdNumber += 1;
            var newMovieNumber = lastLoadedIdNumber.ToString().PadLeft(ImdbApi.MovieIdStartNumber.Length, '0');
            return ImdbApi.MoiveIdCode + newMovieNumber;
        }

        private bool AddNewMovie(string newMovieId)
        {
            return _imdbService.GetMovieByImdbId(newMovieId);
        }
    }
}
