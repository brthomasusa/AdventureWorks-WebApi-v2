using System;
using System.Linq;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
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
            return PagedList<EmployeeDepartmentHistory>.ToPagedList(
                FindByCondition(hist => hist.BusinessEntityID == employeeID),
                deptHistoryParameters.PageNumber,
                deptHistoryParameters.PageSize);
        }

        public EmployeeDepartmentHistory GetDepartmentHistoryByID(int employeeID, int deptID, int shiftID)
        {
            return FindByCondition(hist =>
                   hist.BusinessEntityID == employeeID &&
                   hist.DepartmentID == deptID &&
                   hist.ShiftID == shiftID)
                .FirstOrDefault();
        }

        public void CreateDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            var deptHistory = new EmployeeDepartmentHistory { };
            deptHistory.Map(departmentHistory);

            try
            {
                Create(deptHistory);
                Save();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("{'BusinessEntityID', 'DepartmentID', 'ShiftID', 'StartDate'}", StringComparison.OrdinalIgnoreCase))
                {
                    throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate employee department history record!", ex);
                }
            }
        }

        public void UpdateDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            var deptHistory = DbContext.EmployeeDepartmentHistory.Where(hist =>
                   hist.BusinessEntityID == departmentHistory.BusinessEntityID &&
                   hist.DepartmentID == departmentHistory.DepartmentID &&
                   hist.ShiftID == departmentHistory.ShiftID)
                .FirstOrDefault();

            // The first four fields are part of the primary key,
            // therefore, only the EndDate field can be edited.
            deptHistory.EndDate = departmentHistory.EndDate;
            Update(deptHistory);
            Save();
        }

        public void DeleteDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            var deptHistory = DbContext.EmployeeDepartmentHistory.Where(hist =>
                   hist.BusinessEntityID == departmentHistory.BusinessEntityID &&
                   hist.DepartmentID == departmentHistory.DepartmentID &&
                   hist.ShiftID == departmentHistory.ShiftID)
                .FirstOrDefault();

            Delete(deptHistory);
            Save();
        }
    }
}