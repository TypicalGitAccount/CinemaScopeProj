using System.Web.Mvc;
using AutoMapper;
using Identity.Dtos;
using Identity.Interfaces;
using CinemaScopeWeb.ViewModels;
using CinemaScopeWeb.ViewModels.Account;

namespace CinemaScopeWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountService _accountService;

        public AccountController(IAccountService userService)
        {
            _accountService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userDto = Mapper.Map<RegisterDto>(model);
            var result = _accountService.Register(userDto);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors) 
                    ModelState.AddModelError("", error);
                return View(model);
            }

            _accountService.Login(Mapper.Map<LoginDto>(userDto));
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginUserViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = _accountService.Validate(Mapper.Map<LoginDto>(model));
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            _accountService.Login(Mapper.Map<LoginDto>(model));

            if (_accountService.IsAdministrator)
                return RedirectToAction("Index", "Admin");
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _accountService.Logout();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}