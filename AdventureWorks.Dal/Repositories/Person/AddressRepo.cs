using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class AddressRepo : RepoBase<Address>, IAddressRepo
    {
        public AddressRepo(AdventureWorksContext context) : base(context) { }

        internal AddressRepo(DbContextOptions<AdventureWorksContext> options) : base(options) { }
    }
}