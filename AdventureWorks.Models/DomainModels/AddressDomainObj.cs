using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.SqlServer.Types;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.DomainModels
{
    public class AddressDomainObj : AddressBase
    {
        public int AddressTypeID { get; set; }

        public int ParentEntityID { get; set; }
    }
}