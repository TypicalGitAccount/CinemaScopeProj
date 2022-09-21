using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.Validation
{
    public class PlotValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = (string)value;
            if (val == "")
                return new ValidationResult("This field can't be empty string!");

            return ValidationResult.Success;
        }
    }
}
