using System.ComponentModel.DataAnnotations;
using AdventureWorks.Models.CustomTypes;
using Newtonsoft.Json;

namespace AdventureWorks.Models.Base.Purchasing
{
    public class VendorBase : EntityBase
    {
        [Required]
        public int BusinessEntityID { get; set; } = -1;

        [Required, Display(Name = "Account Number")]
        [StringLength(15, ErrorMessage = "Account number length can't be more than 15 characters.")]
        public string AccountNumber { get; set; }

        [Required, Display(Name = "Vendor Name")]
        [StringLength(50, ErrorMessage = "Vendor name can't be more than 50 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Valid credit ratings are intergers 1 through 5 inclusive.")]
        public CreditRating CreditRating { get; set; }

        [Required, Display(Name = "Preferred Vendor")]
        public bool PreferredVendor { get; set; } = true;

        [Required, Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [DataType(DataType.Url), Display(Name = "Purchasing Web Service URL")]
        public string PurchasingWebServiceURL { get; set; }
    }
}