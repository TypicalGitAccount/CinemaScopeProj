using MovieService.Entities;
using System.Collections.Generic;

namespace MovieService.Interfaces.RepositoryInterfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        void CreateUpdate(Movie movie);
        Movie GetLastUploadedFromImdb();
        Movie GetByImdbId(string ImdbId);
        List<Movie> GetAllNewestFirst();
    }
}
