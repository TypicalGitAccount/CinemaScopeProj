using NUnit.Framework;
using Identity.Interfaces;
using Identity.Services;

namespace CinemaScope.Tests.Identity.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userService = new UserService();
        }


        [Test]
        [TestCase(null, null)]
        [TestCase(null, "password")]
        [TestCase("password", null)]
        public void ChangePassword_NullablePasswords_FailedIdentityResult(string oldPassword, string newPassword)
        {
            var result = _userService.ChangePassword(oldPassword, newPassword);
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        [TestCase("password", "password")]
        public void ChangePassword_EqualsPasswords_FailedIdentityResult(string oldPassword, string newPassword)
        {
            var result = _userService.ChangePassword(oldPassword, newPassword);
            Assert.IsFalse(result.Succeeded);
        }

    }
}
