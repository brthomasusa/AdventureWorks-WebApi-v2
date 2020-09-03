using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AdventureWorks.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class EmailPromoPreferenceAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly List<int> _validEmailPromoCodes = new List<int> { 0, 1, 2 };

        protected override ValidationResult IsValid(object promoPrefCode, ValidationContext validationContext)
        {
            var inputValue = (int)promoPrefCode;

            if (_validEmailPromoCodes.Contains(inputValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Valid email promotion preference codes are 0, 1, and 2.");
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