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
        public PersonPhoneRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public PagedList<PersonPhone> GetPhones(int entityID, PersonPhoneParameters phoneParameters)
        {
            return PagedList<PersonPhone>.ToPagedList(
                FindByCondition(hist => hist.BusinessEntityID == entityID),
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
            var phone = new PersonPhone { };
            phone.Map(telephone);

            try
            {
                Create(phone);
                Save();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("{'BusinessEntityID', 'PhoneNumber', 'PhoneNumberTypeID'}", StringComparison.OrdinalIgnoreCase))
                {
                    throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate phone record!", ex);
                }
            }
        }

        public void UpdatePhone(PersonPhone telephone)
        {
            throw new System.NotImplementedException();
        }

        public void DeletePhone(PersonPhone telephone)
        {
            var phone = DbContext.PersonPhone.Find(telephone.BusinessEntityID, telephone.PhoneNumber, telephone.PhoneNumberTypeID);
            Delete(phone);
            Save();
        }
    }
}