using System.Web.Mvc;
using CinemaScopeWeb.ViewModels;
using Unity;
using Unity.Mvc5;
using MovieService.Interfaces.ServicesInterfaces;
using MovieService.Contexts;
using MovieService.Imdb;
using MovieService.Interfaces;
using MovieService.Repositories;
using MovieService.Services;
using Identity.Interfaces;
using Identity.Services;
using MovieService.Interfaces.ServiceInterfaces;
using PagedList;


namespace CinemaScopeWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<MovieContext>();
            container.RegisterType<MovieTypeRepository>();
            container.RegisterType<GenreRepository>();
            container.RegisterType<CountryRepository>();
            container.RegisterType<CustomHttpClient>();
            container.RegisterType<MovieRepository>();
            container.RegisterType<UserToMovieRepository>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IMovieService, MovieService.Services.MovieService>();

            container.RegisterType<IImdbService, ImdbService>();
            container.RegisterType<MovieService.Interfaces.IUnitOfWork, MovieService.UOW.UnitOfWork>();
            container.RegisterType<Identity.Interfaces.IUnitOfWork, Identity.UOW.UnitOfWork>();
            container.RegisterType<IUserStatsService, UserStatsService>();
            container.RegisterType<IAboutUsService, AboutUsService>();
            container.RegisterType<IFilteringService, FilteringService>();
            container.RegisterType<IImageService, ImageService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}