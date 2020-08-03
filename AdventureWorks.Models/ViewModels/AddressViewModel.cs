using System.ComponentModel.DataAnnotations;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.ViewModel
{
    public class AddressViewModel : AddressBase
    {
        [Required, Display(Name = "EntityID")]
        public int BusinessEntityID { get; set; }

        [Display(Name = "State")]
        public string StateProvinceCode { get; set; }

        [Display(Name = "Country")]
        public string CountryRegionCode { get; set; }

        [Display(Name = "Address Type")]
        public string AddressTypeName { get; set; }

        [Required]
        public int AddressTypeID { get; set; }
    }
}