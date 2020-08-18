using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base.HumanResources;

namespace AdventureWorks.Models.HumanResources
{

    public class EmployeeDepartmentHistory : EmployeeDepartmentHistoryBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Employee EmployeeNavigation { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(DepartmentID))]
        public virtual Department DepartmentNavigation { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ShiftID))]
        public virtual Shift ShiftNavigation { get; set; }
    }
}