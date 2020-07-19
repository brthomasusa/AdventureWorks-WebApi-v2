using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Sales
{
    [Table("SalesTaxRate", Schema = "Sales")]
    public class SalesTaxRate : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesTaxRateID { get; set; }

        public int StateProvinceID { get; set; }

        public TaxType TaxType { get; set; }

        public decimal TaxRate { get; set; }

        public string Name { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(StateProvinceID))]
        public virtual StateProvince StateProvinceNavigation { get; set; }
    }
}