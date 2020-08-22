using System;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IDepartmentRepository
    {
        PagedList<Department> GetDepartments(DepartmentParameters deptParameters);

        Department GetDepartmentByID(int departmentID);

        void CreateDepartment(Department department);

        void UpdateDepartment(Department department);

        void DeleteDepartment(Department department);
    }
}