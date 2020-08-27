using System.ComponentModel.DataAnnotations;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.DomainModels
{
    public class AddressDomainObj : AddressBase
    {
        [Required]
        public int AddressTypeID { get; set; }

        [Required]
        public int ParentEntityID { get; set; }
    }
}