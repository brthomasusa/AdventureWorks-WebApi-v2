using System;
using System.Linq;
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
        private ILoggerManager _logger;
        private readonly string className = "AdventureWorks.Dal.Repositories.HumanResources.DepartmentRepository";

        public DepartmentRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context)
        {
            _logger = logger;
        }

        public PagedList<Department> GetDepartments(DepartmentParameters deptParameters)
        {
            _logger.LogInfo($"{className} - GetDepartments()");
            return PagedList<Department>.ToPagedList(
                FindAll(),
                deptParameters.PageNumber,
                deptParameters.PageSize);
        }

        public Department GetDepartmentByID(int departmentID)
        {
            _logger.LogInfo($"{className} - GetDepartment({departmentID})");
            return FindByCondition(dept => dept.DepartmentID == departmentID)
                .FirstOrDefault();
        }

        public void CreatePayHistory(Department department)
        {

        }

        public void UpdatePayHistory(Department department)
        {

        }

        public void DeletePayHistory(Department department)
        {

        }
    }
}