using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base.Person;

namespace AdventureWorks.Models.Person
{
    [Table("PersonPhone", Schema = "Person")]
    public class PersonPhone : PersonPhoneBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Person PersonNavigation { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(PhoneNumberTypeID))]
        public virtual PhoneNumberType PhoneNumberTypeNavigation { get; set; }
    }
}