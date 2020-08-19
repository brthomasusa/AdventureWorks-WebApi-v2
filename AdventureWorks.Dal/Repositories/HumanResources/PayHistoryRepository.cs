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
    public class PayHistoryRepository : RepositoryBase<EmployeePayHistory>, IPayHistoryRepository
    {
        public PayHistoryRepository(AdventureWorksContext context) : base(context) { }

        public PagedList<EmployeePayHistory> GetPayHistories(int employeeID, PayHistoryParameters payHistoryParameters)
        {
            return PagedList<EmployeePayHistory>.ToPagedList(
                FindByCondition(hist => hist.BusinessEntityID == employeeID),
                payHistoryParameters.PageNumber,
                payHistoryParameters.PageSize);
        }

        public EmployeePayHistory GetPayHistoryByID(int employeeID, DateTime rateChangeDate)
        {
            return FindByCondition(hist =>
                   hist.BusinessEntityID == employeeID &&
                   hist.RateChangeDate == rateChangeDate)
                .FirstOrDefault();
        }

        public void CreatePayHistory(EmployeePayHistory payHistory)
        {
            var history = new EmployeePayHistory { };
            history.Map(payHistory);

            try
            {
                Create(history);
                Save();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("{'BusinessEntityID', 'RateChangeDate'}", StringComparison.OrdinalIgnoreCase))
                {
                    throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate employee pay history record!", ex);
                }
            }
        }

        public void UpdatePayHistory(EmployeePayHistory payHistory)
        {
            var history = DbContext.EmployeePayHistory.Where(hist =>
                   hist.BusinessEntityID == payHistory.BusinessEntityID &&
                   hist.RateChangeDate == payHistory.RateChangeDate)
                .FirstOrDefault();

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

            Delete(history);
            Save();
        }
    }
}