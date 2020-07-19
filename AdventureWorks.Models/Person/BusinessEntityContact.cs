using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("BusinessEntityContact", Schema = "Person")]
    public class BusinessEntityContact : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(AdventureWorks.Models.Person.Person.BusinessEntityID))]        
        public int PersonID { get; set; }   // Foreign key for Person.BusinessEntityID (PK of Person entity)
        public virtual Person PersonNavigation { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContactTypeID { get; set; }
        
        [JsonIgnore]
        [ForeignKey(nameof(ContactTypeID))]
        public virtual ContactType ContactTypeNavigation { get; set; }
    }
}