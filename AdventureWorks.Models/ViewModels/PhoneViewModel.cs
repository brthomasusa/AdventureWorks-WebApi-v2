using System.ComponentModel.DataAnnotations;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.ViewModel
{
    public class PhoneViewModel : EntityBase
    {
        [Required, Display(Name="ID")]
        public int BusinessEntityID { get; set; }

        [Required, DataType(DataType.PhoneNumber), StringLength(25), Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        public int PhoneNumberTypeID { get; set; } 

        [Display(Name="Phone Number Type")]
        public string PhoneNumberType { get; set; }
    }
}