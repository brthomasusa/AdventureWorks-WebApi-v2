using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Models.DomainModels
{
    public class ContactDomainObj : PersonBase
    {
        public int ContactTypeID { get; set; }

        public int ParentEntityID { get; set; }

        [NotMapped]
        public List<PersonPhone> Phones { get; set; } = new List<PersonPhone>();
    }
}