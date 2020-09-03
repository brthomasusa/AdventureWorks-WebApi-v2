using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AdventureWorks.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PersonTypeAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string[] _validTypes = { "SC", "IN", "SP", "EM", "VC", "GC" };

        protected override ValidationResult IsValid(object personType, ValidationContext validationContext)
        {
            var inputValue = personType as string;

            if (Array.Exists(_validTypes, element => element == inputValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Valid values for PersonType are: SC, IN, SP, EM, VC, and GC.");
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