using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IEmployeeRepository
    {
        Task<PagedList<EmployeeDomainObj>> GetEmployees(EmployeeParameters employeeParameters);

        Task<EmployeeDomainObj> GetEmployeeByID(int businessEntityID);

        Task<EmployeeDomainObj> GetEmployeeByIDWithDetails(int businessEntityID);

        Task CreateEmployee(EmployeeDomainObj employeeDomainObj);

        Task UpdateEmployee(EmployeeDomainObj employeeDomainObj);

        Task DeleteEmployee(EmployeeDomainObj employeeDomainObj);
    }
}