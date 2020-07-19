using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;


namespace AdventureWorks.Models.Person
{
    [Table("BusinessEntityAddress", Schema = "Person")]
    public class BusinessEntityAddress : EntityBase
    {
        public int BusinessEntityID { get; set; }

        public int AddressID { get; set; }

        public int AddressTypeID { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(AddressID))]
        public virtual Address AddressNavigation { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(AddressTypeID))]
        public virtual AddressType AddressTypeNavigation { get; set; }
    }
}