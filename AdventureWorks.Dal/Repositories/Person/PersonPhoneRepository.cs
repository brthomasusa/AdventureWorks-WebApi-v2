using System.Linq;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class PersonPhoneRepository : RepositoryBase<PersonPhone>, IPersonPhoneRepository
    {
        public PersonPhoneRepository(AdventureWorksContext context) : base(context) { }

        public PagedList<PersonPhone> GetPhones(int entityID, PersonPhoneParameters phoneParameters)
        {
            return null;
        }

        public PersonPhone GetPhoneByID(int entityID, string phoneNumber, int phoneNumberTypeID)
        {
            return null;
        }

        public void CreatePhone(PersonPhone telephone)
        {

        }

        public void UpdatePhone(PersonPhone telephone)
        {

        }

        public void DeletePhone(PersonPhone telephone)
        {

        }
    }
}