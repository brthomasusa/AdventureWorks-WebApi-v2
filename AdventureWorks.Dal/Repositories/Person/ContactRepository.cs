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
            DoDatabaseValidation(contactDomainObj);

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
            DoDatabaseValidation(contactDomainObj);

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
                throw new AdventureWorksNullEntityObjectException(msg);
            }

        }

        public void DeleteContact(ContactDomainObj contactDomainObj)
        {
            var checkExistence = GetContactByID(contactDomainObj.BusinessEntityID);

            if (checkExistence == null)
            {
                string msg = $"Error: Delete failed; unable to locate a contact in the database with ID '{contactDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteContact " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
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

        private void DoDatabaseValidation(ContactDomainObj contactDomainObj)
        {
            if (!IsValidContactTypeID(contactDomainObj.ContactTypeID))
            {
                var msg = "Error: Invalid contact type detected.";
                RepoLogger.LogError(CLASSNAME + ".DoDatabaseValidation " + msg);
                throw new AdventureWorksInvalidContactTypeException(msg);
            }

            if (!IsValidParentEntityID(contactDomainObj.ParentEntityID, contactDomainObj.PersonType))
            {
                var msg = string.Empty;

                if (contactDomainObj.PersonType == "VC")
                {
                    msg = "Error: Unable to determine the vendor that this contact is to be assigned to.";
                }

                RepoLogger.LogError(CLASSNAME + ".DoDatabaseValidation " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        private bool IsValidContactTypeID(int contactTypeID)
        {
            return DbContext.ContactType.Where(ct => ct.ContactTypeID == contactTypeID).Any();
        }

        private bool IsValidParentEntityID(int entityID, string personType)
        {
            bool retVal = false;

            switch (personType)
            {
                case "VC":
                    retVal = DbContext.Vendor.Where(v => v.BusinessEntityID == entityID).Any();
                    break;
                case "SC":
                    break;
                default:
                    break;
            }

            return retVal;
        }
    }
}