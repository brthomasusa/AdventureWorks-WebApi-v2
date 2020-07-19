using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.HumanResources
{
    [Table("EmployeeDepartmentHistory", Schema = "HumanResources")]
    public class EmployeeDepartmentHistory : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        public short DepartmentID { get; set; }

        public byte ShiftID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }


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