using AutoMapper;
using System.Collections.Generic;
using System.Web.Mvc;
using CinemaScopeWeb.ViewModels;
using Identity.Interfaces;
using Identity.Dtos;

namespace CinemaScopeWeb.Controllers
{
    public class AboutUsController : Controller
    {
        private IAboutUsService _aboutUsService;
        private IImageService _imageService;

        public AboutUsController(IAboutUsService aboutUsService, IImageService imageService)
        {
            _aboutUsService = aboutUsService;
            _imageService = imageService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var users = _aboutUsService.GetAll();
            var model = Mapper.Map<IEnumerable<AboutUsViewModel>>(users);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var model = new CreateAboutUsViewModel();
            model.Image = _imageService.DefaultImage;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(CreateAboutUsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Image = _imageService.DefaultImage;
                return View(model);
            }        

            var user = Mapper.Map<CreateAboutUsDto>(model);
            _aboutUsService.Create(user, Request.Files); 

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var user = _aboutUsService.GetById(id);
            if (user is null) return RedirectToAction("Index");

            var model = Mapper.Map<AboutUsViewModel>(user);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(AboutUsViewModel model)
        {
            if (!ModelState.IsValid)
            {                
                model.Image = _imageService.GetImage(model.Id);
                return View(model);
            }

            var user = Mapper.Map<AboutUsDto>(model);
            _aboutUsService.Update(user, Request.Files);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var user = _aboutUsService.GetById(id);
            if (user is null) return RedirectToAction("Index");

            _aboutUsService.DeleteById(id);
            return RedirectToAction("Index");
        }
    }
}