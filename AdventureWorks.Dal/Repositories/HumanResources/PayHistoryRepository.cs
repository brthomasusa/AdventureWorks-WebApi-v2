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
    public class PayHistoryRepository : RepositoryBase<EmployeePayHistory>, IPayHistoryRepository
    {
        private const string CLASSNAME = "PayHistoryRepository";

        public PayHistoryRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public PagedList<EmployeePayHistory> GetPayHistories(int employeeID, PayHistoryParameters payHistoryParameters)
        {
            if (IsValidEmployeeID(employeeID))
            {
                return PagedList<EmployeePayHistory>.ToPagedList(
                    FindByCondition(hist => hist.BusinessEntityID == employeeID),
                    payHistoryParameters.PageNumber,
                    payHistoryParameters.PageSize);
            }
            else
            {
                var msg = $"Error: '{employeeID}' is not a valid employee ID.";
                RepoLogger.LogError(CLASSNAME + ".GetPayHistories " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        public EmployeePayHistory GetPayHistoryByID(int employeeID, DateTime rateChangeDate)
        {
            if (IsValidEmployeeID(employeeID))
            {
                return FindByCondition(hist =>
                        hist.BusinessEntityID == employeeID &&
                        hist.RateChangeDate == rateChangeDate)
                    .FirstOrDefault();
            }
            else
            {
                var msg = $"Error: '{employeeID}' is not a valid employee ID.";
                RepoLogger.LogError(CLASSNAME + ".GetPayHistoryByID " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        public void CreatePayHistory(EmployeePayHistory payHistory)
        {
            if (IsValidEmployeeID(payHistory.BusinessEntityID))
            {
                if (!IsExistingPayHistory(payHistory))
                {
                    var history = new EmployeePayHistory { };
                    history.Map(payHistory);
                    Create(history);
                    Save();
                }
                else
                {
                    var msg = "Error: This operation would result in a duplicate employee pay history record.";
                    RepoLogger.LogError(CLASSNAME + ".CreatePayHistory " + msg);
                    throw new AdventureWorksUniqueIndexException(msg);
                }
            }
            else
            {
                var msg = $"Error: '{payHistory.BusinessEntityID}' is not a valid employee ID.";
                RepoLogger.LogError(CLASSNAME + ".CreatePayHistory " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }
        }

        public void UpdatePayHistory(EmployeePayHistory payHistory)
        {
            var history = DbContext.EmployeePayHistory.Where(hist =>
                   hist.BusinessEntityID == payHistory.BusinessEntityID &&
                   hist.RateChangeDate == payHistory.RateChangeDate)
                .FirstOrDefault();

            if (history == null)
            {
                string msg = $"Error: Update failed; unable to locate a pay history record in the database with employee ID '{payHistory.BusinessEntityID}' and rate change date '{payHistory.RateChangeDate.ToString()}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdatePayHistory " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            history.Map(payHistory);
            Update(history);
            Save();
        }

        public void DeletePayHistory(EmployeePayHistory payHistory)
        {
            var history = DbContext.EmployeePayHistory.Where(hist =>
                hist.BusinessEntityID == payHistory.BusinessEntityID &&
                hist.RateChangeDate == payHistory.RateChangeDate)
                .FirstOrDefault();

            if (history == null)
            {
                string msg = $"Error: Delete failed; unable to locate a pay history record in the database with employee ID '{payHistory.BusinessEntityID}' and rate change date '{payHistory.RateChangeDate.ToString()}'.";
                RepoLogger.LogError(CLASSNAME + ".DeletePayHistory " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            Delete(history);
            Save();
        }

        private bool IsValidEmployeeID(int employeeID)
        {
            return DbContext.Employee.Where(ph => ph.BusinessEntityID == employeeID).Any();
        }

        private bool IsExistingPayHistory(EmployeePayHistory payHistory)
        {
            return DbContext.EmployeePayHistory
                .Where(ph => ph.BusinessEntityID == payHistory.BusinessEntityID && ph.RateChangeDate == payHistory.RateChangeDate).Any();
        }
    }
}