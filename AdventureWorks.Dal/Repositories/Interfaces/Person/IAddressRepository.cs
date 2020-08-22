using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Person
{
    public interface IAddressRepository
    {
        PagedList<AddressDomainObj> GetAddresses(int entityID, AddressParameters addressParameters);

        AddressDomainObj GetAddressByID(int addressID);

        void CreateAddress(AddressDomainObj addressDomainObj);

        void UpdateAddress(AddressDomainObj addressDomainObj);

        void DeleteAddress(AddressDomainObj addressDomainObj);
    }
}