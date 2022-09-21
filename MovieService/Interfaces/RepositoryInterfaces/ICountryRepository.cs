using System.Collections.Generic;
using MovieService.Entities;

namespace MovieService.Interfaces.RepositoryInterfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country GetByName(string name);
        List<Country> GetRangeByName(List<string> countryNames);
    }
}