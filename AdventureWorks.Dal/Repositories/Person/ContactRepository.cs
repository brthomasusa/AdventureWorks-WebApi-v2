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
    public class ContactRepository : RepositoryBase<ContactDomainObj>, IContactRepository
    {
        public ContactRepository(AdventureWorksContext context) : base(context) { }

        public PagedList<ContactDomainObj> GetContacts(ContactParameters contactParameters)
        {
            return null;
        }

        public ContactDomainObj GetContactByID(int contactID)
        {
            return null;
        }

        public void CreateContact(ContactDomainObj contact)
        {

        }

        public void UpdateContact(ContactDomainObj contact)
        {

        }

        public void DeleteContact(ContactDomainObj contact)
        {

        }
    }
}