using System;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IPayHistoryRepository : IRepositoryBase<EmployeePayHistory>
    {
        PagedList<EmployeePayHistory> GetPayHistories(int employeeID, PayHistoryParameters payHistoryParameters);

        EmployeePayHistory GetPayHistoryByID(int employeeID, DateTime rateChangeDate);

        void CreatePayHistory(EmployeePayHistory payHistory);

        void UpdatePayHistory(EmployeePayHistory payHistory);

        void DeletePayHistory(EmployeePayHistory payHistory);
    }
}