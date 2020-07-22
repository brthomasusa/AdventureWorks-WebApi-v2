using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class BusinessEntityAddressRepo : RepoBase<BusinessEntityAddress>, IBusinessEntityAddressRepo
    {
        public BusinessEntityAddressRepo(AdventureWorksContext context) : base(context) { }

        internal BusinessEntityAddressRepo(DbContextOptions<AdventureWorksContext> options) : base(options) { }        
    }
}