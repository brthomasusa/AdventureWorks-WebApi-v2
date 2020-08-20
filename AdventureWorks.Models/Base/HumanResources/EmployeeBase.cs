using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Models.Base.HumanResources
{
    public class EmployeeBase : EntityBase
    {
        public int BusinessEntityID { get; set; }

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
    }
}