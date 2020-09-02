using System;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IDepartmentHistoryRepository
    {
        PagedList<EmployeeDepartmentHistory> GetDepartmentHistories(int employeeID, DepartmentHistoryParameters deptHistoryParameters);

        EmployeeDepartmentHistory GetDepartmentHistoryByID(int employeeID, short deptID, byte shiftID, DateTime startDate);

        void CreateDepartmentHistory(EmployeeDepartmentHistory departmentHistory);

        void UpdateDepartmentHistory(EmployeeDepartmentHistory departmentHistory);

        void DeleteDepartmentHistory(EmployeeDepartmentHistory departmentHistory);
    }
}