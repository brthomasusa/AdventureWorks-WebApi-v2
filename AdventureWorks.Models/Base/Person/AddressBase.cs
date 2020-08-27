using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.SqlServer.Types;

namespace AdventureWorks.Models.Base
{
    public class AddressBase : EntityBase
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }

        [Required, StringLength(60), Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [StringLength(60), Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required, StringLength(30)]
        public string City { get; set; }

        [Required]
        public int StateProvinceID { get; set; }

        [Required, StringLength(15), Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        public SqlGeography SpatialLocation { get; set; }
    }
}