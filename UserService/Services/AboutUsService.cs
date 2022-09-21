using System.Collections.Generic;
using System.Web;
using System.Linq;
using AutoMapper;
using Identity.Interfaces;
using Identity.Dtos;
using Identity.Models;

namespace Identity.Services
{
    public class AboutUsService : IAboutUsService
    {
        private IUnitOfWork _unitOfWork;
        private IImageService _imageService;

        public AboutUsService(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        /// <summary>
        /// Get all existing AboutUs DTO objects.
        /// </summary>
        /// <returns>IEnumerable of AboutUs DTO objects.</returns>
        public IEnumerable<AboutUsDto> GetAll()
        {
            var users = _unitOfWork.AboutUsRepository.GetAll().ToList();
            var usersDto = Mapper.Map<List<AboutUsDto>>(users);
            foreach (var userDto in usersDto)
                userDto.Image = _imageService.GetImage(userDto.Id);
            return usersDto;
        }

        /// <summary>
        /// Get an existing AboutUs DTO object by its id.
        /// </summary>
        /// <param name="id">AboutUs DTO's id value.</param>
        /// <returns>AboutUs DTO object or null if it doesn't exist.</returns>
        public AboutUsDto GetById(int id)
        {
            var user = _unitOfWork.AboutUsRepository.GetById(id);
            if(user is null) return null;
            var userDto = Mapper.Map<AboutUsDto>(user);
            userDto.Image = _imageService.GetImage(user.Id);
            return userDto;
        }

        /// <summary>
        /// Create a new AboutUs DTO object.
        /// </summary>
        /// <param name="item">CreateAboutUs DTO value.</param>
        /// <param name="files">List of files that were uploaded.</param>
        public void Create(CreateAboutUsDto item, HttpFileCollectionBase files)
        {
            var user = Mapper.Map<AboutUser>(item);
            _unitOfWork.AboutUsRepository.Create(user);
            _unitOfWork.Save();
            var id = GetAll().Last().Id;
            _imageService.CreateImage(id, files);            
        }

        /// <summary>
        /// Update an existing AboutUser object.
        /// </summary>
        /// <param name="item">AboutUs DTO value.</param>
        /// <param name="files">List of files that were uploaded.</param>
        public void Update(AboutUsDto item, HttpFileCollectionBase files)
        {
            var user = Mapper.Map<AboutUser>(item);            
            _unitOfWork.AboutUsRepository.Update(user);
            _unitOfWork.Save();
            _imageService.UpdateImage(item.Id, files);
        }

        /// <summary>
        /// Delete AboutUs DTO object by its id.
        /// </summary>
        /// <param name="id">AboutUs DTO's id value.</param>
        public void DeleteById(int id)
        {
            _unitOfWork.AboutUsRepository.DeleteById(id);
            _unitOfWork.Save();
            _imageService.DeleteImage(id);
        }            
    }
}
