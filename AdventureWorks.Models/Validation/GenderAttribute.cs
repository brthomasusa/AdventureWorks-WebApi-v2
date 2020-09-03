using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AdventureWorks.Models.Validation
{
    public class GenderAttribute : ValidationAttribute, IClientModelValidator
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

        public void AddValidation(ClientModelValidationContext context)
        {
            var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-error", error);
        }
    }
}