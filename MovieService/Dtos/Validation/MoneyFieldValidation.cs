using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.Validation
{
    public class MoneyFieldValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!double.TryParse((string)value, out double result) && !string.IsNullOrEmpty((string)value))
                return new ValidationResult("This field must be a numeric value!");

            return ValidationResult.Success;
        }
    }
}
