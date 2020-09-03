using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
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
        private const string CLASSNAME = "DepartmentRepository";

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
            if (IsDuplicateDeptRecord(department))
            {
                var msg = $"Error: Create department failed; there is already a department named '{department.Name}'.";
                RepoLogger.LogError(CLASSNAME + ".CreateDepartment " + msg);
                throw new AdventureWorksUniqueIndexException(msg);
            }

            var dept = new Department { };
            dept.Map(department);
            Create(dept);
            Save();
            department.DepartmentID = dept.DepartmentID;
        }

        public void UpdateDepartment(Department department)
        {
            if (IsDuplicateDeptRecord(department))
            {
                var msg = "Error: Update department failed; there is another department with this name.";
                RepoLogger.LogError(CLASSNAME + ".UpdateDepartment " + msg);
                throw new AdventureWorksUniqueIndexException(msg);
            }

            var dept = DbContext.Department
                .Where(dept => dept.DepartmentID == department.DepartmentID)
                .FirstOrDefault();

            if (dept == null)
            {
                string msg = $"Error: Update failed; no record found with department ID '{department.DepartmentID}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdateDepartment " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            dept.Map(department);
            Update(dept);
            Save();
        }

        public void DeleteDepartment(Department department)
        {
            var dept = DbContext.Department
                .Where(dept => dept.DepartmentID == department.DepartmentID)
                .FirstOrDefault();

            if (dept == null)
            {
                string msg = $"Error: Delete failed; no record found with department ID '{department.DepartmentID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteDepartment " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            Delete(dept);
            Save();
        }

        private bool IsDuplicateDeptRecord(Department dept)
        {
            var isDuplicate = false;
            var departments = DbContext.Department.ToList();

            foreach (var department in departments)
            {
                if (String.Equals(department.Name, dept.Name, StringComparison.OrdinalIgnoreCase))
                {
                    isDuplicate = (department.DepartmentID != dept.DepartmentID);
                    break;
                }
            }

            return isDuplicate;
        }
    }
}