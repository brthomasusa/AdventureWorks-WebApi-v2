using System;
using System.Linq;
using AdventureWorks.Dal.EfCode;
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
            return null;
        }

        public EmployeePayHistory GetPayHistoryByID(int employeeID, DateTime rateChangeDate)
        {
            return null;
        }

        public void CreatePayHistory(EmployeePayHistory payHistory)
        {

        }

        public void UpdatePayHistory(EmployeePayHistory payHistory)
        {

        }

        public void DeletePayHistory(EmployeePayHistory payHistory)
        {

        }
    }
}