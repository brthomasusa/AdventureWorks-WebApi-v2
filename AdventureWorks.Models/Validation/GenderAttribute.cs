using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Models.Validation
{
    public class GenderAttribute : ValidationAttribute
    {
        private readonly string[] validGenderCodes = { "F", "M" };

        protected override ValidationResult IsValid(object genderCode, ValidationContext validationContext)
        {
            var inputValue = (string)genderCode;

            if (validGenderCodes.Contains(inputValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Valid gender codes are 'F' for female and 'M' for male.");
            }
        }
    }
}