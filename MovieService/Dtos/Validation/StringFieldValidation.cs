using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.Validation
{
    public class StringFieldValidation : ValidationAttribute    
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = (string)value;
            if (string.IsNullOrEmpty(val))
                return new ValidationResult("This field can't be null or empty string!");

            return ValidationResult.Success;
        }
    }
}
