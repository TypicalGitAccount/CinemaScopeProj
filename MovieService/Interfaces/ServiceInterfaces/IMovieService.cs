using System.Collections.Generic;
using System.Web.Mvc;
using MovieService.Dtos;
using MovieService.Entities;

namespace MovieService.Interfaces.ServiceInterfaces
{
    public interface IMovieService
    {
        void DeleteMovie(int id);
        Movie GetMovieById(int id); 
        void CreateUpdate(MovieDto movie);
        SelectList PopulateMovieTypeList(int selected);
        MultiSelectList PopulateGenresList(IEnumerable<int> selected);
        MultiSelectList PopulateCountriesList(IEnumerable<int> selected);
        IEnumerable<ManagedMovieDto> GetManagedMovies();
        List<MostWatchedDto> MostWatched();
        List<MostLikedDto> MostLiked();
        void LikeMovie(string userId, int id);
        void DislikeMovie(string userId, int id);
        void MarkAsWatched(string userId, int id);
        string GetUserRating(int id);
    }
}
