using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Base.Purchasing;

namespace AdventureWorks.Models.Purchasing
{
    [Table("Vendor", Schema = "Purchasing")]
    public class Vendor : VendorBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual BusinessEntity BusinessEntityNavigation { get; set; }

        // Navigation property to dependent entity Person.BusinessEntityContact
        // The navigation is one-way; from Purchasing.Vendor to Person.BusinessEntityContact
        public virtual IList<BusinessEntityContact> BusinessEntityContacts { get; set; } = new List<BusinessEntityContact>();  

        // Navigation property to dependent entity Person.BusinessEntityAddress
        // The navigation is one-way; from Purchasing.Vendor to Person.BusinessEntityAddress
        public virtual IList<BusinessEntityAddress> BusinessEntityAddresses { get; set; } = new List<BusinessEntityAddress>();                 
    }
}