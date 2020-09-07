using System;
using System.Linq;
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

        public PagedList<PersonPhone> GetPhones(int entityID, PersonPhoneParameters phoneParameters)
        {
            return PagedList<PersonPhone>.ToPagedList(
                FindByCondition(ph => ph.BusinessEntityID == entityID),
                phoneParameters.PageNumber,
                phoneParameters.PageSize);
        }

        public PersonPhone GetPhoneByID(int entityID, string phoneNumber, int phoneNumberTypeID)
        {
            return FindByCondition(ph =>
                ph.BusinessEntityID == entityID &&
                ph.PhoneNumber == phoneNumber &&
                ph.PhoneNumberTypeID == phoneNumberTypeID
            ).FirstOrDefault();
        }

        public void CreatePhone(PersonPhone telephone)
        {
            DoDatabaseValidation(telephone, "CreatePhone");

            var phone = new PersonPhone { };
            phone.Map(telephone);
            Create(phone);
            Save();
        }

        public void UpdatePhone(PersonPhone telephone)
        {
            DoDatabaseValidation(telephone, "UpdatePhone");

            var phone = new PersonPhone { };
            phone.Map(telephone);
            Create(phone);
            Save();
        }

        public void DeletePhone(PersonPhone telephone)
        {
            if (IsExistingPhoneRecord(telephone))
            {
                var phone = DbContext.PersonPhone
                    .Where(p => p.BusinessEntityID == telephone.BusinessEntityID &&
                                p.PhoneNumber == telephone.PhoneNumber &&
                                p.PhoneNumberTypeID == telephone.PhoneNumberTypeID)
                    .FirstOrDefault();

                Delete(phone);
                Save();
            }
            else
            {
                var msg = $"The phone record with ID: {telephone.BusinessEntityID}, number: {telephone.PhoneNumber}, and type: {telephone.PhoneNumberTypeID} could not be located in the database.";
                RepoLogger.LogError(msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }
        }

        private void DoDatabaseValidation(PersonPhone telephone, string operation)
        {
            if (!IsValidPersonID(telephone.BusinessEntityID))
            {
                var msg = "Error: Unable to determine what person this phone number should be associated with.";
                RepoLogger.LogError($"{CLASSNAME}.{operation} " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }

            if (!IsValidPhoneNumberTypeID(telephone.PhoneNumberTypeID))
            {
                var msg = $"Error: The PhoneNumberTypeID '{telephone.PhoneNumberTypeID}' is not valid.";
                RepoLogger.LogError($"{CLASSNAME}.{operation} " + msg);
                throw new AdventureWorksInvalidPhoneTypeException(msg);
            }

            if (IsExistingPhoneRecord(telephone))
            {
                var msg = "Error: This operation would result in a duplicate phone record.";
                RepoLogger.LogError($"{CLASSNAME}.{operation} " + msg);
                throw new AdventureWorksUniqueIndexException(msg);
            }
        }

        private bool IsValidPersonID(int entityID)
        {
            return (DbContext.Person.Find(entityID) != null);
        }

        public bool IsExistingPhoneRecord(PersonPhone telephone)
        {
            return (GetPhoneByID(telephone.BusinessEntityID, telephone.PhoneNumber, telephone.PhoneNumberTypeID) != null);
        }

        public bool IsValidPhoneNumberTypeID(int phoneNumberTypeID)
        {
            return (DbContext.PhoneNumberType.Find(phoneNumberTypeID) != null);
        }
    }
}