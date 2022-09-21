using System.Collections.Generic;
using MovieService.Entities;

namespace MovieService.Interfaces.ServicesInterfaces
{
    public interface IFilteringService
    {
        void FilterByCountries(List<string> countries, List<Movie> movies);
        void FilterByGenres(List<string> genres, List<Movie> movies);
        void FilterByType(List<string> types, List<Movie> movies);
        void FilterByYears(List<string> years, List<Movie> movies);
        void FilterByWatched(bool isWatched, List<Movie> movies, string userId);
    }
}