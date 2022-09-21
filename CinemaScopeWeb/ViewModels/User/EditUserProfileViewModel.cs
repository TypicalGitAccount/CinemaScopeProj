using System.ComponentModel.DataAnnotations;

namespace CinemaScopeWeb.ViewModels
{
    public class EditUserProfileViewModel
    {
        [Required]
        [Display(Name = "Your first name:")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Your last name:")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Old Password:")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password:")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords are not the same.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        public string PasswordConfirm { get; set; }
    }
}