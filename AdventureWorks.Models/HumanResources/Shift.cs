using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;


namespace AdventureWorks.Models.HumanResources
{
    [Table("Shift", Schema = "HumanResources")]
    public class Shift : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ShiftID { get; set; }

        public string Name { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }


        [InverseProperty(nameof(EmployeeDepartmentHistory.ShiftNavigation))]
        public virtual List<EmployeeDepartmentHistory> DepartmentHistories { get; set; } = new List<EmployeeDepartmentHistory>();
    }
}