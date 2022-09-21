using Microsoft.AspNet.Identity;
using Identity.Dtos;

namespace Identity.Interfaces
{
    public interface IAccountService
    {
        bool IsAdministrator { get; }

        IdentityResult Register(RegisterDto registerDto);

        void Login(LoginDto loginDto);

        void Logout();

        IdentityResult Validate(LoginDto loginDto);
    }
}
