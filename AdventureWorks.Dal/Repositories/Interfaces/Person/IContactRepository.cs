using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Person
{
    public interface IContactRepository
    {
        PagedList<ContactDomainObj> GetContacts(ContactParameters contactParameters);

        ContactDomainObj GetContactByID(int contactID);

        void CreateContact(ContactDomainObj contact);

        void UpdateContact(ContactDomainObj contact);

        void DeleteContact(ContactDomainObj contact);
    }
}