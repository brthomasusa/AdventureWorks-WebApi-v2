using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Sales;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("CountryRegion", Schema = "Person")]
    public class CountryRegion : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CountryRegionCode { get; set; }

        public string Name { get; set; }


        [InverseProperty(nameof(SalesTerritory.CountryRegionNavigation))]
        public virtual IList<SalesTerritory> SalesTerritories { get; set; } = new List<SalesTerritory>();

        [InverseProperty(nameof(StateProvince.CountryRegionNavigation))]
        public virtual IList<StateProvince> StateProvinces { get; set; } = new List<StateProvince>();
    }
}