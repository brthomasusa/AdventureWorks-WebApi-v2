using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IEmployeeRepository
    {
        PagedList<EmployeeDomainObj> GetEmployees(EmployeeParameters employeeParameters);

        EmployeeDomainObj GetEmployeeByID(int businessEntityID);

        void CreateEmployee(EmployeeDomainObj employeeDomainObj);

        void UpdateEmployee(EmployeeDomainObj employeeDomainObj);

        void DeleteEmployee(EmployeeDomainObj employeeDomainObj);
    }
}