using System;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("BusinessEntity", Schema = "Person")]
    public class BusinessEntity : EntityBase
    {
        public BusinessEntity()
        {
            RowGuid = new Guid();
            ModifiedDate = new DateTime();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BusinessEntityID { get; set; }


        [InverseProperty(nameof(Person.BusinessEntityNavigation))]
        public virtual Person PersonObj { get; set; }

        [InverseProperty(nameof(Vendor.BusinessEntityNavigation))]
        public virtual Vendor VendorObj { get; set; }

        // [InverseProperty(nameof(Store.BusinessEntityNavigation))]
        // public virtual Store StoreObj { get; set; }        
    }
}