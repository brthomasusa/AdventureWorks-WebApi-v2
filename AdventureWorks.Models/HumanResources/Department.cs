using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.HumanResources
{
    [Table("Department", Schema = "HumanResources")]
    public class Department : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short DepartmentID { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }


        [InverseProperty(nameof(EmployeeDepartmentHistory.DepartmentNavigation))]
        public virtual List<EmployeeDepartmentHistory> DepartmentHistories { get; set; } = new List<EmployeeDepartmentHistory>();
    }
}