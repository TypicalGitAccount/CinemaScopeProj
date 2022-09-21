using MovieService.Dtos;
using System.Collections.Generic;

namespace MovieService.Interfaces
{
    public interface IUserStatsService
    {
        IEnumerable<UserStatsMovieDto> GetWatchedMovies(string userId);
        IEnumerable<UserStatsMovieDto> GetLikedMovies(string userId);
        IEnumerable<UserStatsMovieDto> GetDislikedMovies(string userId);
    }
}
