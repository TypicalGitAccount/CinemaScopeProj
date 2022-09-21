using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Web;
using System.Collections.Generic;
using AutoMapper;
using Identity.Managers;
using Identity.Dtos;
using Identity.Interfaces;
using Identity.Models;

namespace Identity.Services
{
    public class AccountService : IAccountService
    {
        private ApplicationUserManager _userManager => 
                HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private IAuthenticationManager _authManager => 
                HttpContext.Current.GetOwinContext().Authentication;

        private const string userRole = "User";
        private const string adminRole = "Administrator";

        private string _userId = String.Empty;

        /// <summary>
        /// Check if the current authorized user is in the "Administrator" role.
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                if(_userId == String.Empty) return false;
                return _userManager.IsInRoleAsync(_userId, adminRole).Result;
            }
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registerDto">Register User DTO value.</param>
        /// <returns>IdentityResult value.</returns>
        public IdentityResult Register(RegisterDto registerDto) 
        {
            var user = Mapper.Map<ApplicationUser>(registerDto);
            IdentityResult result = _userManager.Create(user, registerDto.Password);
            if (result.Succeeded)
                _userManager.AddToRole(user.Id, userRole);
            return result;
        }

        /// <summary>
        /// Log in an account of an exist user.
        /// </summary>
        /// <param name="loginDto">Login User DTO value.</param>
        public void Login(LoginDto loginDto)
        {
            var user = _userManager.Find(loginDto.UserName, loginDto.Password);
            if (user is null) return;
            else _userId = user.Id;

            var claim = _userManager.CreateIdentity(user,
                        DefaultAuthenticationTypes.ApplicationCookie);

            _authManager.SignOut();
            _authManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(30)
            }, claim);
        }

        /// <summary>
        /// Log out an account of the current authorized user.
        /// </summary>
        public void Logout()
        {
            _authManager.SignOut();            
            _userId = String.Empty;
        }

        /// <summary>
        /// Validate a user who tries to log in.
        /// </summary>
        /// <param name="loginDto">Login User DTO value.</param>
        /// <returns>IdentityResult value.</returns>
        public IdentityResult Validate(LoginDto loginDto)
        {
            var user = _userManager.FindByName(loginDto.UserName);
            var errors = new List<string>();

            if (user == null) errors.Add("A user with such user name doesn't exist.");
            else
            {
                if (user.IsBanned) errors.Add("Your account was blocked.");
                if (!_userManager.CheckPassword(user, loginDto.Password)) errors.Add("The password is wrong.");
            }

            if (errors.Count > 0)
                return IdentityResult.Failed(errors.ToArray());

            return IdentityResult.Success;
        }

    }
}
