using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Sales;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("StateProvince", Schema = "Person")]
    public class StateProvince : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateProvinceID { get; set; }

        public string StateProvinceCode { get; set; }

        public string CountryRegionCode { get; set; }

        public bool IsOnlyStateProvince { get; set; }

        public string Name { get; set; }

        public int TerritoryID { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(CountryRegionCode))]
        public virtual CountryRegion CountryRegionNavigation { get; set; }

        [ForeignKey(nameof(TerritoryID))]
        public virtual SalesTerritory SalesTerritoryNavigation { get; set; }

        [InverseProperty(nameof(Address.StateProvinceNavigation))]
        public virtual IList<Address> Addresses { get; set; } = new List<Address>();

        [InverseProperty(nameof(SalesTaxRate.StateProvinceNavigation))]
        public virtual IList<SalesTaxRate> SalesTaxRates { get; set; } = new List<SalesTaxRate>();
    }
}