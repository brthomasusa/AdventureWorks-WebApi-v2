using System.Linq;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.HumanResources
{
    public class EmployeeRepository : RepositoryBase<EmployeeDomainObj>, IEmployeeRepository
    {
        public EmployeeRepository(AdventureWorksContext context) : base(context) { }

        public PagedList<EmployeeDomainObj> GetEmployees(EmployeeParameters employeeParameters)
        {
            return PagedList<EmployeeDomainObj>.ToPagedList(FindAll(),
                employeeParameters.PageNumber,
                employeeParameters.PageSize);
        }

        public EmployeeDomainObj GetEmployeeByID(int businessEntityID)
        {
            return FindByCondition(ee => ee.BusinessEntityID == businessEntityID)
                .DefaultIfEmpty(new EmployeeDomainObj())
                .FirstOrDefault();
        }

        public void CreateEmployee(EmployeeDomainObj employee)
        {
            Create(employee);
        }

        public void UpdateEmployee(EmployeeDomainObj employee)
        {
            Update(employee);
        }

        public void DeleteEmployee(EmployeeDomainObj employee)
        {
            Delete(employee);
        }
    }
}