using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.HumanResources
{
    [Table("JobCandidate", Schema = "HumanResources")]
    public class JobCandidate : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobCandidateID { get; set; }

        public int? BusinessEntityID { get; set; }

        public string Resume { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Employee EmployeeNavigation { get; set; }
    }
}