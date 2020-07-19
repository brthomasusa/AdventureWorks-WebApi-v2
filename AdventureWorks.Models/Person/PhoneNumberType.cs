using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("PhoneNumberType", Schema = "Person")]
    public class PhoneNumberType : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhoneNumberTypeID { get; set; }

        public string Name { get; set; }


        [InverseProperty(nameof(PersonPhone.PhoneNumberTypeNavigation))]
        public virtual IList<PersonPhone> Phones { get; set; } = new List<PersonPhone>();
    }
}