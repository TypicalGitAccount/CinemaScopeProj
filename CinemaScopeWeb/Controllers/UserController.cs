﻿using System.Web.Mvc;
using System.Collections.Generic;
using AutoMapper;
using CinemaScopeWeb.ViewModels;
using MovieService.Interfaces;
using Identity.Dtos;
using Identity.Interfaces;

namespace CinemaScopeWeb.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IUserStatsService _userStatsService;

        public UserController(IUserService userService, IUserStatsService userStats)
        {
            _userService = userService;
            _userStatsService = userStats;
        }

        [HttpGet]        
        public ActionResult Index()
        {
            var model = Mapper.Map<UserProfileViewModel>(_userService.GetProfile());

            var movies = _userStatsService.GetWatchedMovies(_userService.UserId);
            model.WatchedMovies = Mapper.Map<IEnumerable<UserStatsMovieViewModel>>(movies);

            movies = _userStatsService.GetLikedMovies(_userService.UserId);
            model.LikedMovies = Mapper.Map<IEnumerable<UserStatsMovieViewModel>>(movies);

            movies = _userStatsService.GetDislikedMovies(_userService.UserId);
            model.DislikedMovies = Mapper.Map<IEnumerable<UserStatsMovieViewModel>>(movies);

            return View(model);
        }  

        [HttpGet]
        public ActionResult Edit()
        {
            var profile = _userService.GetProfile();
            var model = Mapper.Map<EditUserProfileViewModel>(profile);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserProfileViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var result = _userService.Update(Mapper.Map<EditProfileDto>(model));
            if (result.Succeeded && 
                model.OldPassword != null && 
                model.Password != null)
                result = _userService.ChangePassword(model.OldPassword, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}