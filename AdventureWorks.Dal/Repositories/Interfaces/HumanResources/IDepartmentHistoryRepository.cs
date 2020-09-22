using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IDepartmentHistoryRepository
    {
        Task<PagedList<EmployeeDepartmentHistory>> GetDepartmentHistories(int employeeID, DepartmentHistoryParameters deptHistoryParameters);

        Task<EmployeeDepartmentHistory> GetDepartmentHistoryByID(int employeeID, short deptID, byte shiftID, DateTime startDate);

        Task CreateDepartmentHistory(EmployeeDepartmentHistory departmentHistory);

        Task UpdateDepartmentHistory(EmployeeDepartmentHistory departmentHistory);

        Task DeleteDepartmentHistory(EmployeeDepartmentHistory departmentHistory);
    }
}