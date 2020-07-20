using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base.HumanResources;

namespace AdventureWorks.Models.HumanResources
{
    [Table("Employee", Schema = "HumanResources")]
    public class Employee : EmployeeBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Person.Person PersonNavigation { get; set; }

        [InverseProperty(nameof(EmployeeDepartmentHistory.EmployeeNavigation))]
        public virtual List<EmployeeDepartmentHistory> DepartmentHistories { get; set; } = new List<EmployeeDepartmentHistory>();

        [InverseProperty(nameof(EmployeePayHistory.EmployeeNavigation))]
        public virtual List<EmployeePayHistory> PayHistories { get; set; } = new List<EmployeePayHistory>();

        [InverseProperty(nameof(JobCandidate.EmployeeNavigation))]
        public virtual JobCandidate JobCandidateObj { get; set; }
    }
}