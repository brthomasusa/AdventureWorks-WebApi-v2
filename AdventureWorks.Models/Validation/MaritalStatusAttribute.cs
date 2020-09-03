using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AdventureWorks.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class MaritalStatusAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string[] validMaritalStatus = { "M", "S" };

        protected override ValidationResult IsValid(object maritalStatus, ValidationContext validationContext)
        {
            var inputValue = (string)maritalStatus;

            if (validMaritalStatus.Contains(inputValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Valid marital status codes are 'S' for single and 'M' for married.");
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