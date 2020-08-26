using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Helpers;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class ContactRepository : RepositoryBase<ContactDomainObj>, IContactRepository
    {
        private const string CLASSNAME = "ContactRepository";

        public ContactRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

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
            return DbContext.ContactDomainObj
                .Where(contact => contact.BusinessEntityID == contactID)
                .AsQueryable()
                .FirstOrDefault();
        }

        public ContactDomainObj GetContactByIDWithDetails(int contactID)
        {
            var contact = DbContext.ContactDomainObj
                .Where(contact => contact.BusinessEntityID == contactID)
                .AsQueryable()
                .FirstOrDefault();

            contact.Phones.AddRange(DbContext.PersonPhone.Where(p => p.BusinessEntityID == contactID).ToList());
            return contact;
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
            var contact = DbContext.Person
                .Where(c => c.BusinessEntityID == contactDomainObj.BusinessEntityID)
                .Include(c => c.EmailAddressObj)
                .Include(c => c.PasswordObj)
                .FirstOrDefault();

            if (contact != null)
            {
                contact.Map(contactDomainObj);
                contact.EmailAddressObj.PersonEmailAddress = contactDomainObj.EmailAddress;
                contact.PasswordObj.PasswordHash = contactDomainObj.EmailPasswordHash;
                contact.PasswordObj.PasswordSalt = contactDomainObj.EmailPasswordSalt;
                DbContext.Person.Update(contact);
                Save();
            }
            else
            {
                string msg = $"Error: Update failed; unable to locate a contact in the database with ID '{contactDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdateContact " + msg);
                throw new AdventureWorksInvalidObjectKeyFieldException(msg);
            }

        }

        public void DeleteContact(ContactDomainObj contactDomainObj)
        {
            var checkExistence = GetContactByID(contactDomainObj.BusinessEntityID);

            if (checkExistence == null)
            {
                string msg = $"Error: Delete failed; unable to locate a contact in the database with ID '{contactDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteContact " + msg);
                throw new AdventureWorksInvalidObjectKeyFieldException(msg);
            }

            ExecuteInATransaction(DoWork);

            void DoWork()
            {
                var pword = DbContext.Password.Where(p => p.BusinessEntityID == contactDomainObj.BusinessEntityID).FirstOrDefault();
                if (pword != null)
                {
                    DbContext.Password.Remove(pword);
                    Save();
                }

                var email = DbContext.EmailAddress.Where(e => e.BusinessEntityID == contactDomainObj.BusinessEntityID).FirstOrDefault();
                if (email != null)
                {
                    DbContext.EmailAddress.Remove(email);
                    Save();
                }

                var phones = DbContext.PersonPhone.Where(p => p.BusinessEntityID == contactDomainObj.BusinessEntityID).ToList();
                if (phones != null)
                {
                    DbContext.PersonPhone.RemoveRange(phones);
                    Save();
                }

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