using System.Linq;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.HumanResources
{
    public class DepartmentHistoryRepository : RepositoryBase<EmployeeDepartmentHistory>, IDepartmentHistoryRepository
    {
        public DepartmentHistoryRepository(AdventureWorksContext context) : base(context) { }

        public PagedList<EmployeeDepartmentHistory> GetDepartmentHistories(int employeeID, DepartmentHistoryParameters deptHistoryParameters)
        {
            return null;
        }

        public EmployeeDepartmentHistory GetDepartmentHistoryByID(int employeeID, int deptID, int shiftID)
        {
            return null;
        }

        public void CreateDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {

        }

        public void UpdateDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {

        }

        public void DeleteDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {

        }
    }
}