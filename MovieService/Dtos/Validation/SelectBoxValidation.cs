using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.Validation
{
    public class SelectBoxValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("This field can't be unselected!");

            return ValidationResult.Success;
        }
    }
}
