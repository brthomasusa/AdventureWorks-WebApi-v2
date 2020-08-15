using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Person
{
    public interface IAddressRepository : IRepositoryBase<AddressDomainObj>
    {
        PagedList<AddressDomainObj> GetAddresses(AddressParameters addressParameters);

        AddressDomainObj GetAddressByID(int addressID);

        void CreateAddress(AddressDomainObj address);

        void UpdateAddress(AddressDomainObj address);

        void DeleteAddress(AddressDomainObj address);
    }
}