using MovieService.Dtos;
using MovieService.Interfaces;
using MovieService.Repositories;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace MovieService.Services
{
    public class UserStatsService : IUserStatsService
    {
        private UserToMovieRepository _userMovieRepository;
        private MovieRepository _movieRepository;

        public UserStatsService(UserToMovieRepository userMovieRepo, MovieRepository movieRepo) 
        {
            _userMovieRepository = userMovieRepo; 
            _movieRepository = movieRepo;
        }

        /// <summary>
        /// Get the current user's disliked movies by user's id.
        /// </summary>
        /// <param name="userId">User's id value.</param>
        /// <returns>IEnumerable of disliked movies.</returns>
        public IEnumerable<UserStatsMovieDto> GetDislikedMovies(string userId)
        {
            var moviesDto = _userMovieRepository.GetAllById(userId)
                                             .Where(m => m.IsDisLiked == true)
                                             .Join(_movieRepository.GetAll(),
                                                   um => um.MovieId,
                                                   m => m.Id,
                                                   (um, m) => Mapper.Map<UserStatsMovieDto>(m));
            return moviesDto;
        }

        /// <summary>
        /// Get the current user's liked movies by user's id.
        /// </summary>
        /// <param name="userId">User's id value.</param>
        /// <returns>IEnumerable of liked movies.</returns>
        public IEnumerable<UserStatsMovieDto> GetLikedMovies(string userId)
        {
            var moviesDto = _userMovieRepository.GetAllById(userId)
                                             .Where(m => m.IsLiked == true)
                                             .Join(_movieRepository.GetAll(),
                                                   um => um.MovieId,
                                                   m => m.Id,
                                                   (um, m) => Mapper.Map<UserStatsMovieDto>(m));            
            return moviesDto;
        }

        /// <summary>
        /// Get the current user's watched movies by user's id.
        /// </summary>
        /// <param name="userId">User's id value.</param>
        /// <returns>IEnumerable of watched movies.</returns>
        public IEnumerable<UserStatsMovieDto> GetWatchedMovies(string userId)
        {
            var moviesDto = _userMovieRepository.GetAllById(userId)
                                            .Where(m => m.IsWatched == true)
                                            .Join(_movieRepository.GetAll(),
                                                  um => um.MovieId,
                                                  m => m.Id,
                                                  (um, m) => Mapper.Map<UserStatsMovieDto>(m));
            return moviesDto;
        }
    }
}
