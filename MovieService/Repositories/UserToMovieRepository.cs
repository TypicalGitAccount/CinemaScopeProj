using System.Collections.Generic;
using System.Linq;
using MovieService.Contexts;
using MovieService.Entities;
using MovieService.Interfaces.RepositoryInterfaces;

namespace MovieService.Repositories
{
    public class UserToMovieRepository : Repository<UserToMovie>, IUserToMovieRepository
    {
        public UserToMovieRepository(MovieContext context) : base(context) { }

        public IEnumerable<UserToMovie> GetAllById(string userId)
        {
            return GetAll().Where(m => m.ApplicationUserId == userId);
        }

        public UserToMovie GetOneByUserAndMovieIds(string userId, int movieId)
        {
            return GetAll().FirstOrDefault(x => x.ApplicationUserId == userId && x.MovieId == movieId);
        }
    }
}
