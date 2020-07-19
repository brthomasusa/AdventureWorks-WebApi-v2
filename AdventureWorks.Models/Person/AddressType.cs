using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("AddressType", Schema = "Person")]
    public class AddressType : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressTypeID { get; set; }

        public string Name { get; set; }


        [InverseProperty(nameof(BusinessEntityAddress.AddressTypeNavigation))]
        public virtual IList<BusinessEntityAddress> BusinessEntityAddresses { get; set; } = new List<BusinessEntityAddress>();
    }
}