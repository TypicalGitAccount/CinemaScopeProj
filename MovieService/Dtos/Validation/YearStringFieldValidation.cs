using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.Validation
{
    public class YearStringFieldValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty((string)value))
                return new ValidationResult("This field can't be null or empty string!");

            var parsable = int.TryParse((string)value, out int result);
            if (!parsable)
                return new ValidationResult("This field must be an integer number");

            if (result < 1900 || result > 2022)
                return new ValidationResult("Year must be between 1900 and 2022");

            return ValidationResult.Success;
        }
    }
}
