using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imdb;
using IMDbApiLib.Models;
using MovieService.Entities;
using MovieService.Imdb;
using MovieService.Interfaces;
using MovieService.Interfaces.ServiceInterfaces;
using Newtonsoft.Json;

namespace MovieService.Services
{
    public class ImdbService : IDisposable, IImdbService
    {
        private IUnitOfWork _unitOfWork;
        private CustomHttpClient _webClient;

        public ImdbService(IUnitOfWork unitOfWork,CustomHttpClient webClient)
        {
            _unitOfWork = unitOfWork;
            _webClient = webClient;
        }

        public List<SearchOption> SearchMovie(string searchOption)
        {
            if (string.IsNullOrEmpty(searchOption))
                return null;

            var json = _webClient.GetJson(string.Format(ImdbApi.searchRequest, ImdbApi.apiKey, searchOption));
            _webClient.Dispose();
            Console.WriteLine(json);
            var obj = JsonConvert.DeserializeObject<SearchData>(json);
            var result = new List<SearchOption>(obj.Results.Count);
            result.AddRange(obj.Results.Select(item => new SearchOption(item.Id, item.Title)));

            return result;
        }

        private void MapMovieGenres(List<KeyValueItem> genreList, Movie movie)
        {
            var genreNames = new List<string>(genreList.Count);
            genreNames.AddRange(genreList.Select(genre => genre.Value));
            movie.Genres = _unitOfWork.GenreRepository.GetRangeByName(genreNames);
        }

        private void MapMovieCountries(List<KeyValueItem> countries, Movie movie)
        {
            var countryNames = new List<string>(countries.Count);
            countryNames.AddRange(countries.Select(country => country.Value));
            movie.Countries = _unitOfWork.CountryRepository.GetRangeByName(countryNames);
        }

        private string MapCast(List<ActorShort> cast)
        {
            if (cast == null)
                return null;

            var result = new StringBuilder();
            for (int i = 0; i < 5 && i < cast.Count; i++)
            {
                result.Append(cast[i].Name);
                result.Append("\n");
            }

            return result.ToString();
        }

        private Movie MapTitleDataToMovie(TitleData data)
        {
            if (data == null)
                return null;

            var movie = new Movie
            {
                Title = data.Title,
                ImdbId = data.Id,
                Poster = data.Image,
                Year = data.Year,
                Cast = MapCast(data.ActorList),
                Plot = data.Plot,
                Budget = data.BoxOffice.Budget,
                BoxOffice = data.BoxOffice.CumulativeWorldwideGross,
                RatingIMDb = data.IMDbRating,
                TypeId = _unitOfWork.MovieTypeRepository.GetByName(data.Type)
            };
            MapMovieGenres(data.GenreList, movie);
            MapMovieCountries(data.CountryList, movie);

            return movie;
        }

        public bool GetMovieByImdbId(string movieId)
        {
            if (string.IsNullOrEmpty(movieId)) return false;

            var json = _webClient.GetJson(string.Format(ImdbApi.movieRequest, ImdbApi.apiKey, movieId));

            var movie = JsonConvert.DeserializeObject<TitleData>(json);

            if (movie.ErrorMessage != null) return false;

            if (movie == null || _unitOfWork.MovieRepository.GetByImdbId(movie.Id) != null) return false; 
             
            _unitOfWork.MovieRepository.Add(MapTitleDataToMovie(movie));

            return true;
        }

        public Top250Data GetTop250()
        {
            var json = _webClient.GetJson(string.Format(ImdbApi.top250Request, ImdbApi.apiKey));
            _webClient.Dispose();
            return JsonConvert.DeserializeObject<Top250Data>(json);
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}
