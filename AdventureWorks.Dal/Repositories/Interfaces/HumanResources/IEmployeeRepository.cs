using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IEmployeeRepository : IRepositoryBase<EmployeeDomainObj>
    {
        PagedList<EmployeeDomainObj> GetEmployees(EmployeeParameters employeeParameters);

        EmployeeDomainObj GetEmployeeByID(int businessEntityID);

        void CreateEmployee(EmployeeDomainObj employee);

        void UpdateEmployee(EmployeeDomainObj employee);

        void DeleteEmployee(EmployeeDomainObj employee);
    }
}