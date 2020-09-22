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

        Task CreatePayHistory(EmployeePayHistory payHistory);

        Task UpdatePayHistory(EmployeePayHistory payHistory);

        Task DeletePayHistory(EmployeePayHistory payHistory);
    }
}