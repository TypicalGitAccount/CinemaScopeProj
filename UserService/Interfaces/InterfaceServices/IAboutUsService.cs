using System.Collections.Generic;
using System.Web;
using Identity.Dtos;

namespace Identity.Interfaces
{
    public interface IAboutUsService
    {   
        IEnumerable<AboutUsDto> GetAll();

        AboutUsDto GetById(int id);

        void Create(CreateAboutUsDto item, HttpFileCollectionBase files);

        void Update(AboutUsDto item, HttpFileCollectionBase files);

        void DeleteById(int id);
    }
}
