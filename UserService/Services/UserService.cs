using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using AutoMapper;
using Identity.Interfaces;
using Identity.Dtos;
using Identity.Managers;

namespace Identity.Services
{
    public class UserService : IUserService
    {
        private ApplicationUserManager _userManager => 
                HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private const string adminRole = "Administrator";

        /// <summary>
        /// Get the current user's id.
        /// </summary>
        public string UserId => HttpContext.Current.User.Identity.GetUserId();

        /// <summary>
        /// Get the current user's profile.
        /// </summary>
        /// <returns>The current user's profile DTO.</returns>
        public UserProfileDto GetProfile()
        {
            var user = _userManager.FindById(UserId);
            var userDto = Mapper.Map<UserProfileDto>(user);
            return userDto;
        }

        /// <summary>
        /// Get all exist users' profiles.
        /// </summary>
        /// <returns>IEnumerable of users' profiles.</returns>
        public IEnumerable<ManagableUserDto> GetManagableUsers()
        {
            var users = _userManager.Users.ToList();
            var admins = users.Where(u => _userManager.IsInRole(u.Id, adminRole));
            var usersDto = Mapper.Map<IEnumerable<ManagableUserDto>>(users.Except(admins));
            return usersDto;
        }

        /// <summary>
        /// Change the current user's banned state. If the user is banned, it will be unbanned. And reverse.
        /// </summary>
        /// <param name="userName">UserName value.</param>
        public void ManageBanUserByUserName(string userName)
        {
            var user = _userManager.FindByName(userName);
            var isAdmin = _userManager.IsInRoleAsync(user.Id, adminRole).Result;

            if(!isAdmin)
            {
                user.IsBanned = !user.IsBanned;                
                _userManager.Update(user);
            }
        }

        /// <summary>
        /// Update all properties of a user except password.
        /// </summary>
        /// <param name="userDto">User profile dto to edit the user.</param>
        /// <returns>IdentityResult of updating.</returns>
        /// <exception cref="ArgumentNullException">Cannot edit a user who doesn't exist.</exception>
        public IdentityResult Update(EditProfileDto userDto)
        {
            var user = _userManager.FindById(UserId);
            if (user is null)
                throw new ArgumentNullException("User was not found.");            
            user = Mapper.Map(userDto, user);           
            var result = _userManager.Update(user);
            return result;
        }

        /// <summary>
        /// Update the password of a user.
        /// </summary>
        /// <param name="oldPassword">Old user's password value.</param>
        /// <param name="newPassword">New user's password value.</param>
        /// <returns>IdentityResult of password updating.</returns>
        public IdentityResult ChangePassword(string oldPassword, string newPassword)
        {
            var errors = new List<string>();

            if (oldPassword == null || newPassword == null)
            {
                errors.Add("Passwords must be not required.");
                return IdentityResult.Failed(errors.ToArray());
            }

            if (oldPassword.Equals(newPassword))
            {
                errors.Add("Old and new passwords must be different.");
                return IdentityResult.Failed(errors.ToArray());
            }

            var result = _userManager.ChangePassword(UserId, oldPassword, newPassword);

            return result;
        }
    }
}
