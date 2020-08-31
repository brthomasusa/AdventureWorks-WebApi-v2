using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class MaritalStatusAttribute : ValidationAttribute
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
    }
}