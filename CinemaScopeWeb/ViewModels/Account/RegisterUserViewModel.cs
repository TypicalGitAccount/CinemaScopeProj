using System.ComponentModel.DataAnnotations;

namespace CinemaScopeWeb.ViewModels.Account
{
    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name = "Your first name:")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Your last name:")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Your user name on the site:")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords are not the same.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        public string PasswordConfirm { get; set; }
    }
}