using System.Linq;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class AddressRepository : RepositoryBase<AddressDomainObj>, IAddressRepository
    {
        public AddressRepository(AdventureWorksContext context) : base(context) { }

        public PagedList<AddressDomainObj> GetAddresses(AddressParameters addressParameters)
        {
            return null;
        }

        public AddressDomainObj GetAddressID(int addressID)
        {
            return null;
        }

        public void CreateAddress(AddressDomainObj address)
        {

        }

        public void UpdateAddress(AddressDomainObj address)
        {

        }

        public void DeleteAddress(AddressDomainObj address)
        {

        }
    }
}