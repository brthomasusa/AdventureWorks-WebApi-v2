using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PersonTypeAttribute : ValidationAttribute
    {
        private readonly string[] _validTypes = { "SC", "IN", "SP", "EM", "VC", "GC" };

        protected override ValidationResult IsValid(object personType, ValidationContext validationContext)
        {
            var inputValue = personType as string;
            var contact = (Person.Person)validationContext.ObjectInstance;

            if (Array.Exists(_validTypes, element => element == inputValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Valid values for PersonType are: SC, IN, SP, EM, VC, and GC.");
            }
        }
    }
}