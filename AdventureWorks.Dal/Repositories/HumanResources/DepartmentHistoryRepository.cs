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
    public class DepartmentHistoryRepository : RepositoryBase<EmployeeDepartmentHistory>, IDepartmentHistoryRepository
    {
        private const string CLASSNAME = "DepartmentHistoryRepository";

        public DepartmentHistoryRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public PagedList<EmployeeDepartmentHistory> GetDepartmentHistories(int employeeID, DepartmentHistoryParameters deptHistoryParameters)
        {
            if (IsValidEmployeeID(employeeID))
            {
                return PagedList<EmployeeDepartmentHistory>.ToPagedList(
                    FindByCondition(hist => hist.BusinessEntityID == employeeID),
                    deptHistoryParameters.PageNumber,
                    deptHistoryParameters.PageSize);
            }
            else
            {
                var msg = $"Error: '{employeeID}' is not a valid employee ID.";
                RepoLogger.LogError(CLASSNAME + ".GetDepartmentHistories " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        public EmployeeDepartmentHistory GetDepartmentHistoryByID(int employeeID, short deptID, byte shiftID, DateTime startDate)
        {
            return FindByCondition(hist =>
                   hist.BusinessEntityID == employeeID &&
                   hist.DepartmentID == deptID &&
                   hist.ShiftID == shiftID &&
                   hist.StartDate == startDate)
                .FirstOrDefault();
        }

        public void CreateDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            if (IsValidEmployeeID(departmentHistory.BusinessEntityID))
            {
                var existingRecord = DbContext.EmployeeDepartmentHistory.Where(hist =>
                    hist.BusinessEntityID == departmentHistory.BusinessEntityID &&
                    hist.DepartmentID == departmentHistory.DepartmentID &&
                    hist.ShiftID == departmentHistory.ShiftID &&
                    hist.StartDate == departmentHistory.StartDate)
                    .Any();

                if (!existingRecord)
                {
                    var deptHistory = new EmployeeDepartmentHistory { };
                    deptHistory.Map(departmentHistory);
                    Create(deptHistory);
                    Save();
                }
                else
                {
                    var msg = "Error: This operation would result in a duplicate employee department history record.";
                    RepoLogger.LogError(CLASSNAME + ".CreateDepartmentHistory " + msg);
                    throw new AdventureWorksUniqueIndexException(msg);
                }
            }
            else
            {
                var msg = $"Error: '{departmentHistory.BusinessEntityID}' is not a valid employee ID.";
                RepoLogger.LogError(CLASSNAME + ".CreateDepartmentHistory " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        public void UpdateDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            var deptHistory = DbContext.EmployeeDepartmentHistory.Where(hist =>
                   hist.BusinessEntityID == departmentHistory.BusinessEntityID &&
                   hist.DepartmentID == departmentHistory.DepartmentID &&
                   hist.ShiftID == departmentHistory.ShiftID &&
                   hist.StartDate == departmentHistory.StartDate)
                .FirstOrDefault();

            if (deptHistory != null)
            {
                // The first four fields are part of the primary key,
                // therefore, only the EndDate field can be edited.
                deptHistory.EndDate = departmentHistory.EndDate;
                Update(deptHistory);
                Save();
            }
            else
            {
                string msg = $"Error: Update failed; no record found with employee ID '{departmentHistory.BusinessEntityID}', ";
                msg += $"department ID '{departmentHistory.DepartmentID}', shift ID '{departmentHistory.ShiftID}' ";
                msg += $"and start date '{departmentHistory.StartDate.ToString()}'.";

                RepoLogger.LogError(CLASSNAME + ".UpdateDepartmentHistory " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }
        }

        public void DeleteDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            var deptHistory = DbContext.EmployeeDepartmentHistory.Where(hist =>
                   hist.BusinessEntityID == departmentHistory.BusinessEntityID &&
                   hist.DepartmentID == departmentHistory.DepartmentID &&
                   hist.ShiftID == departmentHistory.ShiftID &&
                   hist.StartDate == departmentHistory.StartDate)
                .FirstOrDefault();

            if (deptHistory == null)
            {
                string msg = $"Error: Delete failed; no record found with employee ID '{departmentHistory.BusinessEntityID}', ";
                msg += $"department ID '{departmentHistory.DepartmentID}', shift ID '{departmentHistory.ShiftID}' ";
                msg += $"and start date '{departmentHistory.StartDate.ToString()}'.";

                RepoLogger.LogError(CLASSNAME + ".DeleteDepartmentHistory " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            Delete(deptHistory);
            Save();
        }

        private bool IsValidEmployeeID(int employeeID)
        {
            return DbContext.Employee.Where(ee => ee.BusinessEntityID == employeeID).Any();
        }
    }
}