using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.CustomTypes;

namespace AdventureWorks.Models.HumanResources
{
    [Table("EmployeePayHistory", Schema = "HumanResources")]
    public class EmployeePayHistory : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        public DateTime RateChangeDate { get; set; }

        public decimal Rate { get; set; }

        public PayFrequency PayFrequency { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Employee EmployeeNavigation { get; set; }
    }
}