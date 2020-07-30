using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Base;

using Newtonsoft.Json;

namespace AdventureWorks.Models.Base.Purchasing
{
    public class VendorBase : EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        [Required, DataType(DataType.Text), Display(Name="Account Number")]
        [StringLength(15, ErrorMessage = "Account number length can't be more than 15 characters.")]
        public string AccountNumber { get; set; }

        [Required, DataType(DataType.Text), Display(Name="Vendor Name")]
        [StringLength(50, ErrorMessage = "Vendor name can't be more than 50 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(1, 5)]
        public CreditRating CreditRating { get; set; }

        [Required, Display(Name="Preferred Vendor")]
        public bool PreferredVendor { get; set; } = true;

        [Required, Display(Name="Active")]
        public bool IsActive { get; set; } = true;

        public string PurchasingWebServiceURL { get; set; }          
    }
}