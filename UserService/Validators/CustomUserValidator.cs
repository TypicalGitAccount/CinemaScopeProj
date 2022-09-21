using Microsoft.AspNet.Identity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Identity.Managers;
using Identity.Models;

namespace Identity.Validators
{
    public class CustomUserValidator : UserValidator<ApplicationUser>
    {
        public CustomUserValidator(ApplicationUserManager manager)
            : base(manager)
        { }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);
            var errors = result.Errors.ToList();

            string namePattern = @"^^[A-Z]{1}(?!.*\-\-)[a-zA-Z\-]{1,29}$";

            if (!Regex.IsMatch(user.FirstName, namePattern) ||
                !Regex.IsMatch(user.LastName, namePattern))
                errors.Add("First and Last names must consist of letters, " +
                    "begin with capital letter and can include \" - \" symbol.");

            if (errors.Count > 0)
                return IdentityResult.Failed(errors.ToArray());
            return IdentityResult.Success;
        }
    }
}

