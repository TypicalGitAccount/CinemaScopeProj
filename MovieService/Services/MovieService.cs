using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MovieService.Dtos;
using MovieService.Entities;
using MovieService.Interfaces;
using MovieService.Interfaces.ServiceInterfaces;

namespace MovieService.Services
{
    public class MovieService : IMovieService
    {
        IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Movie GetMovieById(int id)
        {
            return _unitOfWork.MovieRepository.GetById(id);
        }

        public void CreateUpdate(MovieDto movieDto)
        {
            var movie = Mapper.Map<Movie>(movieDto);

            movie.Genres = new List<Genre>();
            foreach(var id in movieDto.GenreIds)
                movie.Genres.Add(_unitOfWork.GenreRepository.GetById(id));

            movie.Countries = new List<Country>();
            foreach(var id in movieDto.CountryIds)
                movie.Countries.Add(_unitOfWork.CountryRepository.GetById(id));

            _unitOfWork.MovieRepository.CreateUpdate(movie);
        }

        /// <summary>
        /// Delete movie by movie's id.
        /// </summary>
        /// <param name="id">Movie's id value.</param>
        public void DeleteMovie(int id)
        {
            _unitOfWork.MovieRepository.DeleteById(id);            
        }

        /// <summary>
        /// Get movies to manage by admin.
        /// </summary>
        /// <returns>IEnumerable of managed movies.</returns>
        public IEnumerable<ManagedMovieDto> GetManagedMovies()
        {
            var movies = _unitOfWork.MovieRepository.GetAll();
            var moviesDto = Mapper.Map<IEnumerable<ManagedMovieDto>>(movies);
            return moviesDto;
        }

        public List<MostWatchedDto> MostWatched()
        {
            return _unitOfWork.UserToMovieRepository.GetAll()
                .Where(m => m.IsWatched)
                .GroupBy(m => m.MovieId)
                .Select(m =>
                    {
                        var movie = _unitOfWork.MovieRepository.GetById(m.Key);
                        return new MostWatchedDto()
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            Plot = movie.Plot,
                            Image = movie.Poster,
                            Watched = m.Count()
                        };
                    }
                ).OrderByDescending(m => m.Watched)
                .ToList();
        }
        public List<MostLikedDto> MostLiked()
        {
            return _unitOfWork.UserToMovieRepository.GetAll()
                .Where(m => m.IsLiked)
                .GroupBy(m => m.MovieId)
                .Select(m =>
                    {
                        var movie = _unitOfWork.MovieRepository.GetById(m.Key);
                        return new MostLikedDto()
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            Plot = movie.Plot,
                            Image = movie.Poster,
                            Liked = m.Count()
                        };
                    }
                ).OrderByDescending(m => m.Liked)
                .ToList();
        }
        public void LikeMovie(string userId, int id)
        {
            var userToMovie = _unitOfWork.UserToMovieRepository.GetOneByUserAndMovieIds(userId, id);
            if (userToMovie is null)
            {
                userToMovie = new UserToMovie()
                {
                    IsDisLiked = false,
                    ApplicationUserId = userId,
                    IsLiked = true,
                    IsWatched = false,
                    MovieId = id
                };
                _unitOfWork.UserToMovieRepository.Add(userToMovie);
            }
            else if (userToMovie.IsLiked)
            {
                userToMovie.IsLiked = false;
                _unitOfWork.UserToMovieRepository.Update(userToMovie);
                if (!userToMovie.IsWatched)
                    _unitOfWork.UserToMovieRepository.Delete(userToMovie);
            }
            else
            {
                if (userToMovie.IsDisLiked)
                    userToMovie.IsDisLiked = false;
                userToMovie.IsLiked = true;
                _unitOfWork.UserToMovieRepository.Update(userToMovie);
            }
        }
        public void DislikeMovie(string userId, int id)
        {
            var userToMovie = _unitOfWork.UserToMovieRepository.GetOneByUserAndMovieIds(userId, id);
            if (userToMovie is null)
            {
                userToMovie = new UserToMovie()
                {
                    IsDisLiked = true,
                    ApplicationUserId = userId,
                    IsLiked = false,
                    IsWatched = false,
                    MovieId = id
                };
                _unitOfWork.UserToMovieRepository.Add(userToMovie);
            }
            else if (userToMovie.IsDisLiked)
            {
                userToMovie.IsDisLiked = false;
                _unitOfWork.UserToMovieRepository.Update(userToMovie);
                if (!userToMovie.IsWatched)
                    _unitOfWork.UserToMovieRepository.Delete(userToMovie);
            }
            else
            {
                if (userToMovie.IsLiked)
                    userToMovie.IsLiked = false;
                userToMovie.IsDisLiked = true;
                _unitOfWork.UserToMovieRepository.Update(userToMovie);
            }
        }
        public void MarkAsWatched(string userId, int id)
        {
            var userToMovie = _unitOfWork.UserToMovieRepository.GetOneByUserAndMovieIds(userId, id);

            if (userToMovie is null)
            {
                userToMovie = new UserToMovie()
                {
                    IsDisLiked = false,
                    ApplicationUserId = userId,
                    IsLiked = false,
                    IsWatched = true,
                    MovieId = id
                };
                _unitOfWork.UserToMovieRepository.Add(userToMovie);
            }
            else if (userToMovie.IsWatched)
            {
                userToMovie.IsWatched = false;
                _unitOfWork.UserToMovieRepository.Update(userToMovie);
                if (!userToMovie.IsDisLiked && !userToMovie.IsLiked)
                    _unitOfWork.UserToMovieRepository.Delete(userToMovie);
            }
            else
            {
                userToMovie.IsWatched = true;
                _unitOfWork.UserToMovieRepository.Update(userToMovie);
            }
        }

        public string GetUserRating(int id)
        {
            var allLikes = _unitOfWork.UserToMovieRepository.GetAll().Count(x => x.IsLiked && x.MovieId==id);
            var allDislikes = _unitOfWork.UserToMovieRepository.GetAll().Count(x => x.IsDisLiked && x.MovieId==id);
            return (allDislikes + allLikes) == 0 ? "0,0" : (10*allLikes/(allDislikes+allLikes)).ToString("0.0");
        }

        public SelectList PopulateMovieTypeList(int selected)
        {
            return new SelectList(_unitOfWork.MovieTypeRepository.GetAll().ToList(), "Id", "Name", selected);
        }

        public MultiSelectList PopulateGenresList(IEnumerable<int> selected)
        {
            return new MultiSelectList(_unitOfWork.GenreRepository.GetAll().ToList(), "Id", "Name", selected);
        }

        public MultiSelectList PopulateCountriesList(IEnumerable<int> selected)
        {
            return new MultiSelectList(_unitOfWork.CountryRepository.GetAll().ToList(), "Id", "Name", selected);
        }
    }
}
