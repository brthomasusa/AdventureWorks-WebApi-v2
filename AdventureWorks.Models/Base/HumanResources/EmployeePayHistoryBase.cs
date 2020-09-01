using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AdventureWorks.Models.CustomTypes;

namespace AdventureWorks.Models.Base.HumanResources
{
    [Table("EmployeePayHistory", Schema = "HumanResources")]
    public class EmployeePayHistoryBase : EntityBase
    {
        [Required]
        public int BusinessEntityID { get; set; }

        [Required]
        public DateTime RateChangeDate { get; set; }

        [Required, Range(7.50, 200.00)]
        public decimal Rate { get; set; }

        [Required, Range(1, 2, ErrorMessage = "1 - Salary rcvd monthly, 2 - Salary rcvd biweekly")]
        public PayFrequency PayFrequency { get; set; }
    }
}