using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("Password", Schema = "Person")]
    public class PersonPWord : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Person PersonNavigation { get; set; }
    }
}