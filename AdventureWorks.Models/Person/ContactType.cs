using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("ContactType", Schema = "Person")]
    public class ContactType : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactTypeID { get; set; }

        public string Name { get; set; }


        [InverseProperty(nameof(BusinessEntityContact.ContactTypeNavigation))]
        public virtual IList<BusinessEntityContact> BusinessEntityContacts { get; set; } = new List<BusinessEntityContact>();
    }
}