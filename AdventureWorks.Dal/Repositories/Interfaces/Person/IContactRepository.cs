using System.Threading.Tasks;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Person
{
    public interface IContactRepository
    {
        Task<PagedList<ContactDomainObj>> GetContacts(int entityID, ContactParameters contactParameters);

        Task<ContactDomainObj> GetContactByID(int contactID);

        Task<ContactDomainObj> GetContactByIDWithPhones(int contactID);

        Task CreateContact(ContactDomainObj contactDomainObj);

        Task UpdateContact(ContactDomainObj contactDomainObj);

        Task DeleteContact(ContactDomainObj contactDomainObj);
    }
}