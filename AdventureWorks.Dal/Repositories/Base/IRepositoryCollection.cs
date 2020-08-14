using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;

namespace AdventureWorks.Dal.Repositories.Base
{
    public interface IRepositoryCollection
    {
        IVendorRepository Vendor { get; }

        IEmployeeRepository Employee { get; }
    }
}