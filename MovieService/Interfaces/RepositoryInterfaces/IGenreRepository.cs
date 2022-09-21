using System.Collections.Generic;
using MovieService.Entities;

namespace MovieService.Interfaces.RepositoryInterfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        List<Genre> GetRangeByName(List<string> genreNames);
    }
}