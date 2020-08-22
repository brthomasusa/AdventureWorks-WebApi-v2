using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Helpers;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.HumanResources
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public PagedList<Department> GetDepartments(DepartmentParameters deptParameters)
        {
            return PagedList<Department>.ToPagedList(
                FindAll(),
                deptParameters.PageNumber,
                deptParameters.PageSize);
        }

        public Department GetDepartmentByID(int departmentID)
        {
            return DbContext.Department
                .Where(dept => dept.DepartmentID == departmentID)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public void CreateDepartment(Department department)
        {
            var dept = new Department { };
            dept.Map(department);
            Create(dept);
            Save();
            department.DepartmentID = dept.DepartmentID;
        }

        public void UpdateDepartment(Department department)
        {
            var dept = DbContext.Department
                .Where(dept => dept.DepartmentID == department.DepartmentID)
                .FirstOrDefault();

            dept.Map(department);
            Update(dept);
            Save();
        }

        public void DeleteDepartment(Department department)
        {
            var dept = DbContext.Department
                .Where(dept => dept.DepartmentID == department.DepartmentID)
                .FirstOrDefault();

            Delete(dept);
            Save();
        }
    }
}