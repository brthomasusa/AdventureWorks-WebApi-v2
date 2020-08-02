using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("EmailAddress", Schema = "Person")]
    public class EmailAddress : EntityBase
    {
        public int BusinessEntityID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailAddressID { get; set; }

        [DataType(DataType.EmailAddress), Display(Name = "Email Address")]
        public string PersonEmailAddress { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Person PersonNavigation { get; set; }
    }
}