using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Helpers;


namespace AdventureWorks.Dal.Repositories.Interfaces.Person
{
    public interface IPersonPhoneRepository
    {
        Task<PagedList<PersonPhone>> GetPhones(int entityID, PersonPhoneParameters phoneParameters);

        Task<PersonPhone> GetPhoneByID(int entityID, string phoneNumber, int phoneNumberTypeID);

        Task CreatePhone(PersonPhone telephone);

        Task UpdatePhone(PersonPhone telephone);

        Task DeletePhone(PersonPhone telephone);
    }
}