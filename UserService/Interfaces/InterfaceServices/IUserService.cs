using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Identity.Dtos;

namespace Identity.Interfaces
{
    public interface IUserService
    {
        string UserId { get; }

        UserProfileDto GetProfile();

        void ManageBanUserByUserName(string userName);

        IEnumerable<ManagableUserDto> GetManagableUsers();

        IdentityResult Update(EditProfileDto userDto);

        IdentityResult ChangePassword(string oldPassword, string newPassword);
    }
}
