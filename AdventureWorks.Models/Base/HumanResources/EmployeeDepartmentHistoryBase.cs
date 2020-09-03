using System;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Validation;

namespace AdventureWorks.Models.Base.HumanResources
{
    [Table("EmployeeDepartmentHistory", Schema = "HumanResources")]
    public class EmployeeDepartmentHistoryBase : EntityBase
    {
        public int BusinessEntityID { get; set; }

        public short DepartmentID { get; set; }

        public byte ShiftID { get; set; }

        public DateTime StartDate { get; set; }

        [DateGreaterThan("StartDate", ErrorMessage = "End date must be greater than start date.")]
        public DateTime? EndDate { get; set; }
    }
}