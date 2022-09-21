using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.Validation
{
    public class PosterLinkValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = (string)value;

            if (string.IsNullOrEmpty(val))
                return new ValidationResult("This field can't be null or empty string!");

            if (!val.Contains("http"))
                return new ValidationResult("This field must be a link!");

            return ValidationResult.Success;
        }
    }
}
