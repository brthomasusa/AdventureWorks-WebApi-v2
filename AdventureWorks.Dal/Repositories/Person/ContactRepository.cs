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

        public PagedList<ContactDomainObj> GetContacts(int entityID, ContactParameters contactParameters)
        {
            return PagedList<ContactDomainObj>.ToPagedList(
                DbContext.ContactDomainObj
                    .Where(contact => contact.ParentEntityID == entityID)
                    .AsQueryable(),
                contactParameters.PageNumber,
                contactParameters.PageSize);
        }

        public ContactDomainObj GetContactByID(int contactID)
        {
            return DbContext.ContactDomainObj.Where(contact => contact.BusinessEntityID == contactID)
                .AsQueryable()
                .FirstOrDefault();
        }

        public void CreateContact(ContactDomainObj contactDomainObj)
        {
            ExecuteInATransaction(DoWork);

            void DoWork()
            {
                var bizEntity = new BusinessEntity { };
                DbContext.BusinessEntity.Add(bizEntity);
                Save();

                contactDomainObj.BusinessEntityID = bizEntity.BusinessEntityID;
                var contact = new AdventureWorks.Models.Person.Person { };
                contact.Map(contactDomainObj);

                contact.EmailAddressObj = new EmailAddress
                {
                    BusinessEntityID = bizEntity.BusinessEntityID,
                    PersonEmailAddress = contactDomainObj.EmailAddress
                };

                contact.PasswordObj = new PersonPWord
                {
                    PasswordHash = contactDomainObj.EmailPasswordHash,
                    PasswordSalt = contactDomainObj.EmailPasswordSalt
                };

                DbContext.Person.Add(contact);
                Save();

                var bec = new BusinessEntityContact
                {
                    BusinessEntityID = contactDomainObj.ParentEntityID,
                    PersonID = contact.BusinessEntityID,
                    ContactTypeID = contactDomainObj.ContactTypeID
                };
                DbContext.BusinessEntityContact.Add(bec);
                Save();
            }
        }

        public void UpdateContact(ContactDomainObj contactDomainObj)
        {
            var contact = DbContext.Person.Find(contactDomainObj.BusinessEntityID);
            contact.Map(contactDomainObj);
            contact.EmailAddressObj.PersonEmailAddress = contactDomainObj.EmailAddress;
            contact.PasswordObj.PasswordHash = contactDomainObj.EmailPasswordHash;
            contact.PasswordObj.PasswordSalt = contactDomainObj.EmailPasswordSalt;
            DbContext.Person.Update(contact);
            Save();
        }

        public void DeleteContact(ContactDomainObj contactDomainObj)
        {
            ExecuteInATransaction(DoWork);

            void DoWork()
            {
                var bec = DbContext.BusinessEntityContact
                    .Find(contactDomainObj.ParentEntityID, contactDomainObj.BusinessEntityID, contactDomainObj.ContactTypeID);

                if (bec != null)
                {
                    DbContext.BusinessEntityContact.Remove(bec);
                    Save();
                }

                var contact = DbContext.Person.Find(contactDomainObj.BusinessEntityID);
                DbContext.Person.Remove(contact);
                Save();

                var bizEntity = DbContext.BusinessEntity.Find(contactDomainObj.BusinessEntityID);
                DbContext.BusinessEntity.Remove(bizEntity);
                Save();
            }
        }
    }
}