using System.Linq;
using System.Threading.Tasks;
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

        public async Task<PagedList<ContactDomainObj>> GetContacts(int entityID, ContactParameters contactParameters)
        {
            if (DbContext.BusinessEntity.Where(v => v.BusinessEntityID == entityID).Any())
            {
                var pagedList = await PagedList<ContactDomainObj>.ToPagedList(
                    DbContext.ContactDomainObj
                        .Where(contact => contact.ParentEntityID == entityID)
                        .AsQueryable()
                        .OrderBy(p => p.LastName)
                        .ThenBy(p => p.FirstName)
                        .ThenBy(p => p.MiddleName),
                    contactParameters.PageNumber,
                    contactParameters.PageSize);

                return pagedList;
            }
            else
            {
                var msg = "Error: Invalid BusinessEntityID detected.";
                RepoLogger.LogError(CLASSNAME + ".GetContacts " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        public async Task<ContactDomainObj> GetContactByID(int contactID)
        {
            return await DbContext.ContactDomainObj
                .Where(contact => contact.BusinessEntityID == contactID)
                .FirstOrDefaultAsync();
        }

        public async Task<ContactDomainObj> GetContactByIDWithPhones(int contactID)
        {
            if (await DbContext.ContactDomainObj.Where(p => p.BusinessEntityID == contactID).AnyAsync())
            {
                var contact = await DbContext.ContactDomainObj
                    .Where(contact => contact.BusinessEntityID == contactID)
                    .FirstOrDefaultAsync();

                contact.Phones.AddRange(await DbContext.PersonPhone.Where(p => p.BusinessEntityID == contactID).ToListAsync());
                return contact;
            }
            else
            {
                var msg = "Error: Invalid ContactID detected.";
                RepoLogger.LogError(CLASSNAME + ".GetContactByIDWithPhones " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        public async Task CreateContact(ContactDomainObj contactDomainObj)
        {
            await DoDatabaseValidation(contactDomainObj);

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {

                    var bizEntity = new BusinessEntity { };
                    DbContext.BusinessEntity.Add(bizEntity);
                    await Save();

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
                    await Save();

                    var bec = new BusinessEntityContact
                    {
                        BusinessEntityID = contactDomainObj.ParentEntityID,
                        PersonID = contact.BusinessEntityID,
                        ContactTypeID = contactDomainObj.ContactTypeID
                    };
                    DbContext.BusinessEntityContact.Add(bec);
                    await Save();

                    transaction.Commit();

                }
                catch (System.Exception ex)
                {
                    RepoLogger.LogError($" {CLASSNAME}.CreateContact {ex.Message}");
                }
            }
        }

        public async Task UpdateContact(ContactDomainObj contactDomainObj)
        {
            await DoDatabaseValidation(contactDomainObj);

            var contact = await DbContext.Person
                .Where(c => c.BusinessEntityID == contactDomainObj.BusinessEntityID)
                .Include(c => c.EmailAddressObj)
                .Include(c => c.PasswordObj)
                .FirstOrDefaultAsync();

            if (contact != null)
            {
                contact.Map(contactDomainObj);
                contact.EmailAddressObj.PersonEmailAddress = contactDomainObj.EmailAddress;
                contact.PasswordObj.PasswordHash = contactDomainObj.EmailPasswordHash;
                contact.PasswordObj.PasswordSalt = contactDomainObj.EmailPasswordSalt;
                DbContext.Person.Update(contact);

                await Save();
            }
            else
            {
                string msg = $"Error: Update failed; unable to locate a contact in the database with ID '{contactDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdateContact " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }
        }

        public async Task DeleteContact(ContactDomainObj contactDomainObj)
        {

            if (await DbContext.Person.Where(p => p.BusinessEntityID == contactDomainObj.BusinessEntityID).AnyAsync())
            {
                string msg = $"Error: Delete failed; unable to locate a contact in the database with ID '{contactDomainObj.BusinessEntityID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteContact " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {

                    var pword = DbContext.Password.Where(p => p.BusinessEntityID == contactDomainObj.BusinessEntityID).FirstOrDefault();
                    if (pword != null)
                    {
                        DbContext.Password.Remove(pword);
                        await Save();
                    }

                    var email = DbContext.EmailAddress.Where(e => e.BusinessEntityID == contactDomainObj.BusinessEntityID).FirstOrDefault();
                    if (email != null)
                    {
                        DbContext.EmailAddress.Remove(email);
                        await Save();
                    }

                    var phones = DbContext.PersonPhone.Where(p => p.BusinessEntityID == contactDomainObj.BusinessEntityID).ToList();
                    if (phones != null)
                    {
                        DbContext.PersonPhone.RemoveRange(phones);
                        await Save();
                    }

                    var bec = DbContext.BusinessEntityContact
                        .Find(contactDomainObj.ParentEntityID, contactDomainObj.BusinessEntityID, contactDomainObj.ContactTypeID);

                    if (bec != null)
                    {
                        DbContext.BusinessEntityContact.Remove(bec);
                        await Save();
                    }

                    var contact = await DbContext.Person.FindAsync(contactDomainObj.BusinessEntityID);
                    DbContext.Person.Remove(contact);
                    await Save();

                    var bizEntity = DbContext.BusinessEntity.Find(contactDomainObj.BusinessEntityID);
                    DbContext.BusinessEntity.Remove(bizEntity);
                    await Save();

                    transaction.Commit();

                }
                catch (System.Exception ex)
                {
                    RepoLogger.LogError($" {CLASSNAME}.DeleteContact {ex.Message}");
                }
            }
        }

        private async Task DoDatabaseValidation(ContactDomainObj contactDomainObj)
        {
            if (await IsValidContactTypeID(contactDomainObj.ContactTypeID) == false)
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

        private async Task<bool> IsValidContactTypeID(int contactTypeID)
        {
            return await DbContext.ContactType.Where(ct => ct.ContactTypeID == contactTypeID).AnyAsync();
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