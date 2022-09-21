using MovieService.Contexts;
using MovieService.Entities;
using System.Data.Entity;
using System.Linq;
using MovieService.Interfaces.RepositoryInterfaces;
using System.Collections.Generic;

namespace MovieService.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieContext context) : base(context) { }

        public void CreateUpdate(Movie movie)
        {
            var oldMovie = ((MovieContext)_context).Movies.FirstOrDefault(m => m.Id == movie.Id);
            if (oldMovie != null)
            {
                oldMovie.Title = movie.Title;
                oldMovie.ImdbId = movie.ImdbId;
                oldMovie.Year = movie.Year;
                oldMovie.Poster = movie.Poster;
                oldMovie.Plot = movie.Plot;
                oldMovie.Countries.Clear();
                oldMovie.Countries = movie.Countries;
                oldMovie.Genres.Clear();
                oldMovie.Genres = movie.Genres;
                oldMovie.Budget = movie.Budget;
                oldMovie.BoxOffice = movie.BoxOffice;
                oldMovie.Cast = movie.Cast;
                oldMovie.TypeId = movie.TypeId;
            } else
            {
                ((MovieContext)_context).Movies.Add(movie);
            }
            Save();
        }

        public List<Movie> GetAllNewestFirst()
        {
            return GetAll().OrderByDescending(m => m.Year).ToList();
        }

        public override Movie GetById(int id)
        {
            var movie = ((MovieContext)_context).Movies.
                Where(m => m.Id == id).Include((m => m.Genres)).Include(m => m.Countries).FirstOrDefault();

            return movie;
        }

        public override void Delete(Movie item)
        {
            var repository = new Repository<UserToMovie>(_context);
            var movies = repository.GetAll().Where(um => um.MovieId == item.Id);
            foreach (var movie in movies)
            {
                repository.Delete(movie);
            }
            Save();
            base.Delete(item);
        }

        public override void DeleteById(int id)
        {
            var repository = new Repository<UserToMovie>(_context);
            var movies = repository.GetAll().Where(um => um.MovieId == id);
            foreach (var movie in movies)
            {
                repository.Delete(movie);
            }
            Save();
            base.DeleteById(id);
        }

        public Movie GetLastUploadedFromImdb()
        {
            return ((MovieContext)_context).Movies.OrderByDescending(m => m.Id).Where(m => m.ImdbId != null).FirstOrDefault();
        }

        public Movie GetByImdbId(string ImdbId)
        {
            if (string.IsNullOrEmpty(ImdbId)) { return null; }

            return ((MovieContext)_context).Movies.FirstOrDefault(m => m.ImdbId == ImdbId);
        }
    }
}
