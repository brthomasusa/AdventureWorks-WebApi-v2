using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class BusinessEntityRepo : RepoBase<BusinessEntity>, IBusinessEntityRepo
    {
        public BusinessEntityRepo(AdventureWorksContext context) : base(context) { }

        internal BusinessEntityRepo(DbContextOptions<AdventureWorksContext> options) : base(options) { }
    }
}