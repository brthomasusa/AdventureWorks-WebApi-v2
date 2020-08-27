using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("Address", Schema = "Person")]
    public class Address : AddressBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(StateProvinceID))]
        public virtual StateProvince StateProvinceNavigation { get; set; }

        [InverseProperty(nameof(BusinessEntityAddress.AddressNavigation))]
        public virtual BusinessEntityAddress BusinessEntityAddressObj { get; set; }
    }
}