using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Helpers;


namespace AdventureWorks.Dal.Repositories.Interfaces.Person
{
    public interface IPersonPhoneRepository : IRepositoryBase<PersonPhone>
    {
        PagedList<PersonPhone> GetPhones(int entityID, PersonPhoneParameters phoneParameters);

        PersonPhone GetPhoneByID(int entityID, string phoneNumber, int phoneNumberTypeID);

        void CreatePhone(PersonPhone telephone);

        void UpdatePhone(PersonPhone telephone);

        void DeletePhone(PersonPhone telephone);
    }
}