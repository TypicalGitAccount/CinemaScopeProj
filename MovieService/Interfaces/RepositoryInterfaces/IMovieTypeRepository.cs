using MovieService.Entities;

namespace MovieService.Interfaces.RepositoryInterfaces
{
    public interface IMovieTypeRepository : IRepository<MovieType>
    { 
        int GetByName(string movieTypeName);
    }
}