using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base.Purchasing;

namespace AdventureWorks.Models.DomainModels
{
    public class VendorDomainObj : VendorBase
    {
        [NotMapped]
        public IList<AddressDomainObj> Addresses { get; set; } = new List<AddressDomainObj>();

        [NotMapped]
        public IList<ContactDomainObj> Contacts { get; set; } = new List<ContactDomainObj>();
    }
}