using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Dal.Repositories.Utility;

namespace AdventureWorks.Dal.Repositories.Base
{
    public interface IRepositoryCollection
    {
        IVendorRepository Vendor { get; }

        IEmployeeRepository Employee { get; }

        IDepartmentHistoryRepository DepartmentHistory { get; }

        IPayHistoryRepository PayHistory { get; }

        IPersonPhoneRepository Telephone { get; }

        IDepartmentRepository Department { get; }

        IAddressRepository Address { get; }

        IContactRepository Contact { get; }

        ILookupRepository Lookup { get; }
    }
}