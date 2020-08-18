using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base.HumanResources;

namespace AdventureWorks.Models.HumanResources
{

    public class EmployeePayHistory : EmployeePayHistoryBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Employee EmployeeNavigation { get; set; }
    }
}