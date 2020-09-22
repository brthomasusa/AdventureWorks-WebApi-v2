using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Person
{
    public interface IAddressRepository
    {
        Task<PagedList<AddressDomainObj>> GetAddresses(int entityID, AddressParameters addressParameters);

        Task<AddressDomainObj> GetAddressByID(int addressID);

        Task CreateAddress(AddressDomainObj addressDomainObj);

        Task UpdateAddress(AddressDomainObj addressDomainObj);

        Task DeleteAddress(AddressDomainObj addressDomainObj);
    }
}