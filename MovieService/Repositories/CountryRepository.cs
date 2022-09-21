using MovieService.Contexts;
using MovieService.Entities;
using System.Collections.Generic;
using System.Linq;
using MovieService.Interfaces.RepositoryInterfaces;

namespace MovieService.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(MovieContext context) : base(context) { }

        public Country GetByName(string name)
        {
            return ((MovieContext)_context).Countries.FirstOrDefault(c => c.Name == name);
        }

        /// <summary>
        /// Gets range of countries by names, creates if doesnt exist
        /// </summary>
        /// <param name="countryNames"></param>
        /// <param name="movie"></param>
        /// <returns>list of countries</returns>
        public List<Country> GetRangeByName(List<string> countryNames)
        {
            if (countryNames == null || countryNames.Count == 0)    
                return null;

            var countries = new List<Country>();
            foreach (var countryName in countryNames)
            {
                var country = GetByName(countryName);
                if (country == null)
                {
                    country = new Country();
                    country.Name = countryName;
                    Add(country);
                    Save();
                }
                Update(country);
                Save();
                countries.Add(country);
            }

            return countries;
        }
    }
}
