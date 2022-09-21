using MovieService.Entities;

namespace MovieService.Interfaces.RepositoryInterfaces
{
    public interface IUserToMovieRepository : IRepository<UserToMovie>
    {
        UserToMovie GetOneByUserAndMovieIds(string userId, int movieId);
    }
}