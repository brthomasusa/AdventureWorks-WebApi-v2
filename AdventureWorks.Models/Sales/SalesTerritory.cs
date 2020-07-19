using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Sales
{
    [Table("SalesTerritory", Schema = "Sales")]
    public class SalesTerritory : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TerritoryID { get; set; }

        public string Name { get; set; }

        [Required]
        public string CountryRegionCode { get; set; }

        public string Group { get; set; }

        public decimal SalesYTD { get; set; }

        public decimal SalesLastYear { get; set; }

        public decimal CostYTD { get; set; }

        public decimal CostLastYear { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(CountryRegionCode))]
        public virtual CountryRegion CountryRegionNavigation { get; set; }

        [InverseProperty(nameof(StateProvince.SalesTerritoryNavigation))]
        public virtual IList<StateProvince> StateProvinces { get; set; } = new List<StateProvince>();
    }
}