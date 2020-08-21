using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Base.Person
{
    public class PersonPhoneBase : EntityBase
    {
        public int BusinessEntityID { get; set; }

        [Required, DataType(DataType.PhoneNumber), StringLength(25), Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public int PhoneNumberTypeID { get; set; }
    }
}