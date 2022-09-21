using MovieService.Contexts;
using MovieService.Entities;
using System;
using System.Linq;
using MovieService.Interfaces.RepositoryInterfaces;

namespace MovieService.Repositories
{
    public class MovieTypeRepository : Repository<MovieType>, IMovieTypeRepository
    {

        public MovieTypeRepository(MovieContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets movie type id by name, creates if doesnt exist
        /// </summary>
        /// <param name="movieTypeName"></param>
        /// <returns>id of movie type</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public int GetByName(string movieTypeName)
        {
            if (string.IsNullOrEmpty(movieTypeName))
                throw new ArgumentNullException("movieTypeName can't be null/empty string!");

            var movieType = GetAll().SingleOrDefault(t => t.Name == movieTypeName);
            if (movieType != null) return movieType.Id;
            movieType = new MovieType
            {
                Name = movieTypeName
            };
            Add(movieType);
            Save();
            return movieType.Id;
        }
    }
}
