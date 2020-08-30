using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.Person
{
    public interface IContactRepository
    {
        PagedList<ContactDomainObj> GetContacts(int entityID, ContactParameters contactParameters);

        ContactDomainObj GetContactByID(int contactID);

        ContactDomainObj GetContactByIDWithPhones(int contactID);

        void CreateContact(ContactDomainObj contactDomainObj);

        void UpdateContact(ContactDomainObj contactDomainObj);

        void DeleteContact(ContactDomainObj contactDomainObj);
    }
}