using AutoMapper;
using CinemaScopeWeb.ViewModels;
using CinemaScopeWeb.ViewModels.Account;
using Identity.Dtos;
using Identity.Models;
using MovieService.Dtos;
using MovieService.Entities;

namespace CinemaScopeWeb
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
            Mapper.CreateMap<ApplicationUser, LoginDto>().ReverseMap();
            Mapper.CreateMap<ApplicationUser, UserProfileDto>().ReverseMap();
            Mapper.CreateMap<ApplicationUser, ManagableUserDto>().ReverseMap();
            Mapper.CreateMap<ApplicationUser, EditProfileDto>().ReverseMap();

            Mapper.CreateMap<RegisterDto, LoginDto>().ReverseMap();
            Mapper.CreateMap<RegisterDto, RegisterUserViewModel>().ReverseMap();

            Mapper.CreateMap<LoginDto, LoginUserViewModel>().ReverseMap();

            Mapper.CreateMap<UserProfileDto, UserProfileViewModel>().ReverseMap();
            Mapper.CreateMap<UserProfileDto, EditUserProfileViewModel>().ReverseMap();

            Mapper.CreateMap<EditProfileDto, EditUserProfileViewModel>().ReverseMap();

            Mapper.CreateMap<AboutUsDto, AboutUser>().ReverseMap();
            Mapper.CreateMap<AboutUsDto, AboutUsViewModel>().ReverseMap();

            Mapper.CreateMap<CreateAboutUsDto, AboutUser>().ReverseMap();
            Mapper.CreateMap<CreateAboutUsDto, CreateAboutUsViewModel>().ReverseMap();

            Mapper.CreateMap<Movie, ManagedMovieDto>().ReverseMap();
            Mapper.CreateMap<Movie, UserStatsMovieDto>().ReverseMap();
            Mapper.CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.TypeId, opt => opt.Ignore())
                .ForMember(dest => dest.MovieTypes, opt => opt.Ignore())
                .ForMember(dest => dest.GenreIds, opt => opt.Ignore())
                .ForMember(dest => dest.CountryIds, opt => opt.Ignore())
                .ForMember(dest => dest.GenreList, opt => opt.Ignore())
                .ForMember(dest => dest.CountriesList, opt => opt.Ignore());
            Mapper.CreateMap<MovieDto, Movie>();

            Mapper.CreateMap<ManagableUserDto, ManagableUserViewModel>().ReverseMap();
            Mapper.CreateMap<ManagedMovieDto, ManagedMovieViewModel>().ReverseMap();   
            Mapper.CreateMap<UserStatsMovieDto, UserStatsMovieViewModel>().ReverseMap();
        }
    }
}