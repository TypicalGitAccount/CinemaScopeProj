using System.Collections.Generic;

namespace MovieService.Interfaces.RepositoryInterfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);

        IEnumerable<T> GetAll();

        void Add(T item);

        void Update(T item);

        void DeleteById(int id);

        void Delete(T item);
    }
}
