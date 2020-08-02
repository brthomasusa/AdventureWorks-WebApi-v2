using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("PersonPhone", Schema = "Person")]
    public class PersonPhone : EntityBase
    {
        public int BusinessEntityID { get; set; }

        [Required, DataType(DataType.PhoneNumber), StringLength(25), Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public int PhoneNumberTypeID { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Person PersonNavigation { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(PhoneNumberTypeID))]
        public virtual PhoneNumberType PhoneNumberTypeNavigation { get; set; }
    }
}