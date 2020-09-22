using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class DepartmentHistoryRepository : RepositoryBase<EmployeeDepartmentHistory>, IDepartmentHistoryRepository
    {
        private const string CLASSNAME = "DepartmentHistoryRepository";

        public DepartmentHistoryRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public async Task<PagedList<EmployeeDepartmentHistory>> GetDepartmentHistories(int employeeID, DepartmentHistoryParameters deptHistoryParameters)
        {
            if (await IsValidEmployeeID(employeeID))
            {
                var pagedList = await PagedList<EmployeeDepartmentHistory>.ToPagedList(
                    DbContext.EmployeeDepartmentHistory
                        .AsNoTracking()
                        .Where(hist => hist.BusinessEntityID == employeeID)
                        .OrderBy(hist => hist.StartDate)
                        .AsQueryable(),
                    deptHistoryParameters.PageNumber,
                    deptHistoryParameters.PageSize);

                return pagedList;
            }
            else
            {
                var msg = $"Error: '{employeeID}' is not a valid employee ID.";
                RepoLogger.LogError(CLASSNAME + ".GetDepartmentHistories " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        public async Task<EmployeeDepartmentHistory> GetDepartmentHistoryByID(int employeeID, short deptID, byte shiftID, DateTime startDate)
        {
            return await DbContext.EmployeeDepartmentHistory
                .Where(hist =>
                   hist.BusinessEntityID == employeeID &&
                   hist.DepartmentID == deptID &&
                   hist.ShiftID == shiftID &&
                   hist.StartDate == startDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task CreateDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            if (await IsValidEmployeeID(departmentHistory.BusinessEntityID))
            {
                var existingRecord = await DbContext.EmployeeDepartmentHistory.Where(hist =>
                    hist.BusinessEntityID == departmentHistory.BusinessEntityID &&
                    hist.DepartmentID == departmentHistory.DepartmentID &&
                    hist.ShiftID == departmentHistory.ShiftID &&
                    hist.StartDate == departmentHistory.StartDate)
                    .AnyAsync();

                if (!existingRecord)
                {
                    var deptHistory = new EmployeeDepartmentHistory { };
                    deptHistory.Map(departmentHistory);
                    Create(deptHistory);
                    await Save();
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

        public async Task UpdateDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            var deptHistory = await DbContext.EmployeeDepartmentHistory.Where(hist =>
                   hist.BusinessEntityID == departmentHistory.BusinessEntityID &&
                   hist.DepartmentID == departmentHistory.DepartmentID &&
                   hist.ShiftID == departmentHistory.ShiftID &&
                   hist.StartDate == departmentHistory.StartDate)
                .FirstOrDefaultAsync();

            if (deptHistory != null)
            {
                // The first four fields are part of the primary key,
                // therefore, only the EndDate field can be edited.
                deptHistory.EndDate = departmentHistory.EndDate;
                Update(deptHistory);
                await Save();
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

        public async Task DeleteDepartmentHistory(EmployeeDepartmentHistory departmentHistory)
        {
            var deptHistory = await DbContext.EmployeeDepartmentHistory.Where(hist =>
                   hist.BusinessEntityID == departmentHistory.BusinessEntityID &&
                   hist.DepartmentID == departmentHistory.DepartmentID &&
                   hist.ShiftID == departmentHistory.ShiftID &&
                   hist.StartDate == departmentHistory.StartDate)
                .FirstOrDefaultAsync();

            if (deptHistory == null)
            {
                string msg = $"Error: Delete failed; no record found with employee ID '{departmentHistory.BusinessEntityID}', ";
                msg += $"department ID '{departmentHistory.DepartmentID}', shift ID '{departmentHistory.ShiftID}' ";
                msg += $"and start date '{departmentHistory.StartDate.ToString()}'.";

                RepoLogger.LogError(CLASSNAME + ".DeleteDepartmentHistory " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            Delete(deptHistory);
            await Save();
        }

        private async Task<bool> IsValidEmployeeID(int employeeID)
        {
            return await DbContext.Employee.Where(ee => ee.BusinessEntityID == employeeID).AnyAsync();
        }
    }
}