using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Models.Base.HumanResources
{
    [Table("EmployeeDepartmentHistory", Schema = "HumanResources")]
    public class EmployeeDepartmentHistoryBase : EntityBase
    {
        public int BusinessEntityID { get; set; }

        public short DepartmentID { get; set; }

        public byte ShiftID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}