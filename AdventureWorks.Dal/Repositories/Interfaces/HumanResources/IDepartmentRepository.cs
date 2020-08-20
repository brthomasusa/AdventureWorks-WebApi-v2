using System;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IDepartmentRepository : IRepositoryBase<Department>
    {
        PagedList<Department> GetDepartments(DepartmentParameters deptParameters);

        Department GetDepartmentByID(int departmentID);

        void CreatePayHistory(Department department);

        void UpdatePayHistory(Department department);

        void DeletePayHistory(Department department);
    }
}