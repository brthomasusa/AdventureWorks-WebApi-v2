using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Helpers;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class PersonPhoneRepository : RepositoryBase<PersonPhone>, IPersonPhoneRepository
    {
        private const string CLASSNAME = "PersonPhoneRepository";

        public PersonPhoneRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public async Task<PagedList<PersonPhone>> GetPhones(int entityID, PersonPhoneParameters phoneParameters)
        {
            var pagedList = await PagedList<PersonPhone>.ToPagedList(
                FindByCondition(ph => ph.BusinessEntityID == entityID),
                phoneParameters.PageNumber,
                phoneParameters.PageSize);

            return pagedList;
        }

        public async Task<PersonPhone> GetPhoneByID(int entityID, string phoneNumber, int phoneNumberTypeID)
        {
            return await DbContext.PersonPhone.Where(ph =>
                ph.BusinessEntityID == entityID &&
                ph.PhoneNumber == phoneNumber &&
                ph.PhoneNumberTypeID == phoneNumberTypeID)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task CreatePhone(PersonPhone telephone)
        {
            await DoDatabaseValidation(telephone, "CreatePhone");

            var phone = new PersonPhone { };
            phone.Map(telephone);
            Create(phone);
            await Save();
        }

        public async Task UpdatePhone(PersonPhone telephone)
        {
            await DoDatabaseValidation(telephone, "UpdatePhone");

            var phone = new PersonPhone { };
            phone.Map(telephone);
            Create(phone);
            await Save();
        }

        public async Task DeletePhone(PersonPhone telephone)
        {
            if (await IsExistingPhoneRecord(telephone))
            {
                var phone = await DbContext.PersonPhone
                    .Where(p => p.BusinessEntityID == telephone.BusinessEntityID &&
                                p.PhoneNumber == telephone.PhoneNumber &&
                                p.PhoneNumberTypeID == telephone.PhoneNumberTypeID)
                    .FirstOrDefaultAsync();

                Delete(phone);
                await Save();
            }
            else
            {
                var msg = $"The phone record with ID: {telephone.BusinessEntityID}, number: {telephone.PhoneNumber}, and type: {telephone.PhoneNumberTypeID} could not be located in the database.";
                RepoLogger.LogError(msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }
        }

        private async Task DoDatabaseValidation(PersonPhone telephone, string operation)
        {
            if (await IsValidPersonID(telephone.BusinessEntityID) == false)
            {
                var msg = "Error: Unable to determine what person this phone number should be associated with.";
                RepoLogger.LogError($"{CLASSNAME}.{operation} " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }


            if (await IsValidPhoneNumberTypeID(telephone.PhoneNumberTypeID) == false)
            {
                var msg = $"Error: The PhoneNumberTypeID '{telephone.PhoneNumberTypeID}' is not valid.";
                RepoLogger.LogError($"{CLASSNAME}.{operation} " + msg);
                throw new AdventureWorksInvalidPhoneTypeException(msg);
            }

            if (await IsExistingPhoneRecord(telephone))
            {
                var msg = "Error: This operation would result in a duplicate phone record.";
                RepoLogger.LogError($"{CLASSNAME}.{operation} " + msg);
                throw new AdventureWorksUniqueIndexException(msg);
            }
        }

        private async Task<bool> IsValidPersonID(int entityID)
        {
            return await DbContext.Person.Where(p => p.BusinessEntityID == entityID).AnyAsync();
        }

        public async Task<bool> IsExistingPhoneRecord(PersonPhone telephone)
        {
            return (await GetPhoneByID(telephone.BusinessEntityID, telephone.PhoneNumber, telephone.PhoneNumberTypeID) != null);
        }

        public async Task<bool> IsValidPhoneNumberTypeID(int phoneNumberTypeID)
        {
            return await DbContext.PhoneNumberType.Where(ph => ph.PhoneNumberTypeID == phoneNumberTypeID).AnyAsync();
        }
    }
}