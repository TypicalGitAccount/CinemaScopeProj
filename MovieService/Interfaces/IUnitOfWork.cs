using System;
using MovieService.Interfaces.RepositoryInterfaces;

namespace MovieService.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICountryRepository CountryRepository { get; }
        IGenreRepository GenreRepository { get; }
        IMovieTypeRepository MovieTypeRepository { get; }
        IMovieRepository MovieRepository { get; }
        IUserToMovieRepository UserToMovieRepository { get; }

    }
}
