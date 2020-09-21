using System;
using System.Threading.Tasks;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Helpers;

namespace AdventureWorks.Dal.Repositories.Interfaces.HumanResources
{
    public interface IPayHistoryRepository
    {
        Task<PagedList<EmployeePayHistory>> GetPayHistories(int employeeID, PayHistoryParameters payHistoryParameters);

        Task<EmployeePayHistory> GetPayHistoryByID(int employeeID, DateTime rateChangeDate);

        void CreatePayHistory(EmployeePayHistory payHistory);

        void UpdatePayHistory(EmployeePayHistory payHistory);

        void DeletePayHistory(EmployeePayHistory payHistory);
    }
}