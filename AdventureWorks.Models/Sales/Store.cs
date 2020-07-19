using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Sales
{
    [Table("Store", Schema = "Sales")]
    public class Store : EntityBase
    {

    }
}