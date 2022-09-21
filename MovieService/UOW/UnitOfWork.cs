using MovieService.Contexts;
using MovieService.Interfaces;
using MovieService.Interfaces.RepositoryInterfaces;
using MovieService.Repositories;

namespace MovieService.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private MovieContext _context;

        private ICountryRepository _countryRepository;
        private IGenreRepository _genreRepository;
        private IMovieTypeRepository _movieTypeRepository;
        private IMovieRepository _movieRepository;
        private IUserToMovieRepository _userToMovieRepository;

        public UnitOfWork(MovieContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public ICountryRepository CountryRepository =>
            _countryRepository ?? (_countryRepository = new CountryRepository(_context));
        public IGenreRepository GenreRepository =>
            _genreRepository ?? (_genreRepository = new GenreRepository(_context));
        public IMovieTypeRepository MovieTypeRepository =>
            _movieTypeRepository ?? (_movieTypeRepository = new MovieTypeRepository(_context));
        public IMovieRepository MovieRepository =>
            _movieRepository ?? (_movieRepository = new MovieRepository(_context));

        public IUserToMovieRepository UserToMovieRepository =>
            _userToMovieRepository ?? (_userToMovieRepository = new UserToMovieRepository(_context));
    }
}
