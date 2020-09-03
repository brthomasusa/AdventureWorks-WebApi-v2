using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base.HumanResources;
using Newtonsoft.Json;

namespace AdventureWorks.Models.HumanResources
{
    [Table("Department", Schema = "HumanResources")]
    public class Department : DepartmentBase
    {
        [JsonIgnore]
        [InverseProperty(nameof(EmployeeDepartmentHistory.DepartmentNavigation))]
        public virtual List<EmployeeDepartmentHistory> DepartmentHistories { get; set; } = new List<EmployeeDepartmentHistory>();
    }
}