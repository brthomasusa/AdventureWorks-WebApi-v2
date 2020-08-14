using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Models.DomainModels
{
    public class EmployeeDomainObj : PersonBase
    {
        [DataType(DataType.EmailAddress), Display(Name = "Email Address")]
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

        [Required, DataType(DataType.Text), StringLength(1, ErrorMessage = "M = married and S = single.")]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [Required, DataType(DataType.Text), StringLength(1, ErrorMessage = "F = female and M = male.")]
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
    }
}