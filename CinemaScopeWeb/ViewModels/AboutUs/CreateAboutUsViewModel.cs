using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace CinemaScopeWeb.ViewModels
{
    public class CreateAboutUsViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name:")]
        [RegularExpression(@"^(?!.*\s\s)(?!.*\.\.)(?!.*,,)[A-Z][a-zA-Z .,]{2,60}$",
            ErrorMessage = "Names must consist of letters and " +
                    "begin with capital letter.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description:")]
        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}