using System;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.CustomTypes;

namespace AdventureWorks.Models.Base.HumanResources
{
    [Table("EmployeePayHistory", Schema = "HumanResources")]
    public class EmployeePayHistoryBase : EntityBase
    {
        public int BusinessEntityID { get; set; }

        public DateTime RateChangeDate { get; set; }

        public decimal Rate { get; set; }

        public PayFrequency PayFrequency { get; set; }
    }
}