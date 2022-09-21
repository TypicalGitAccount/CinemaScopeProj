using MovieService.Contexts;
using MovieService.Entities;
using System.Collections.Generic;
using System.Linq;
using MovieService.Interfaces.RepositoryInterfaces;

namespace MovieService.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {

        public GenreRepository(MovieContext context) : base(context) { }
        
        private Genre GetByName(string name)
        {
            return ((MovieContext)_context).Genres.FirstOrDefault(g => g.Name == name);
        }

        /// <summary>
        /// Gets range of genres by given names, creates if dont exist
        /// </summary>
        /// <param name="genreNames"></param>
        /// <param name="movie"></param>
        /// <returns>list of genres</returns>
        public List<Genre> GetRangeByName(List<string> genreNames)
        {
            if (genreNames == null || genreNames.Count == 0)
                return null;

            var genres = new List<Genre>();
            foreach (var name in genreNames)
            {
                var genre = GetByName(name);
                if (genre == null)
                {
                    genre = new Genre
                    {
                        Name = name
                    };
                    Add(genre);
                    Save();
                }              
                genres.Add(genre);
            }

            return genres;
        }
    }
}
