using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Models.DomainModels;

namespace AdventureWorks.Dal.Repositories.HumanResources
{
    public class EmployeeRepository : RepositoryBase<EmployeeDomainObj>, IEmployeeRepository
    {
        public EmployeeRepository(AdventureWorksContext context) : base(context) { }
    }
}