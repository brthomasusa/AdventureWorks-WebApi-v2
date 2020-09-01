using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Validation;

namespace AdventureWorks.Models.DomainModels
{
    public class EmployeeDomainObj : PersonBase, IValidatableObject
    {
        private readonly DateTime EARLIEST_HIREDATE = new DateTime(1996, 7, 1);
        private readonly DateTime EARLIEST_BIRTHDATE = new DateTime(1930, 1, 1);

        [Required, DataType(DataType.EmailAddress), Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        [Required, DataType(DataType.Text), StringLength(15, ErrorMessage = "National ID number has maximum of 15 characters.")]
        [Display(Name = "National ID Number")]
        public string NationalIDNumber { get; set; }

        [Required, DataType(DataType.Text), StringLength(256, ErrorMessage = "Login ID has a maximum of 256 characters.")]
        [Display(Name = "Login ID")]
        public string LoginID { get; set; }

        [Required, DataType(DataType.Text), StringLength(50, ErrorMessage = "Job title has a mzximum of 50 characters.")]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required, MaritalStatus, MinLength(1), MaxLength(1)]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [Required, Gender, MinLength(1), MaxLength(1)]
        public string Gender { get; set; }

        [Required, DataType(DataType.DateTime), Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Required, Display(Name = "Salaried?")]
        public bool IsSalaried { get; set; }

        [Required, Display(Name = "Vacation Hrs"), Range(-40, 240)]
        public short VacationHours { get; set; } = 0;

        [Required, Display(Name = "Sick Leave Hrs"), Range(0, 120)]
        public short SickLeaveHours { get; set; } = 0;

        [Required, DataType(DataType.DateTime), Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required, Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public List<EmployeeDepartmentHistory> DepartmentHistories { get; set; } = new List<EmployeeDepartmentHistory>();

        [NotMapped]
        public List<EmployeePayHistory> PayHistories { get; set; } = new List<EmployeePayHistory>();

        [NotMapped]
        public List<AddressDomainObj> Addresses { get; set; } = new List<AddressDomainObj>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var maxHireDate = (DateTime.Now).AddDays(1);
            var maxBirthDate = (DateTime.Now).AddYears(-18);

            if (PersonType != "EM")
            {
                yield return new ValidationResult("PersonType should be EM (Employee).", new[] { nameof(PersonType) });
            }

            if (HireDate < EARLIEST_HIREDATE || HireDate > maxHireDate)
            {
                yield return new ValidationResult("Hire date cannot be before 1996-07-01 or after tomorrow.", new[] { nameof(HireDate) });
            }

            if (BirthDate < EARLIEST_BIRTHDATE || BirthDate > maxBirthDate)
            {
                yield return new ValidationResult("Employee must be at least 18 and born on, or after, 1930-01-01.", new[] { nameof(BirthDate) });
            }
        }
    }
}