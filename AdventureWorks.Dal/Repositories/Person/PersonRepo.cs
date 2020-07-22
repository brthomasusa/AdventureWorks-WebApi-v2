using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class PersonRepo : RepoBase<AdventureWorks.Models.Person.Person>, IPersonRepo
    {
        public PersonRepo(AdventureWorksContext context) : base(context) { }

        internal PersonRepo(DbContextOptions<AdventureWorksContext> options) : base(options) { }
    }
}