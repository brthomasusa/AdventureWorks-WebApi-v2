using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.HumanResources;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Retrieve
{
    [Collection("AdventureWorks.Dal")]
    public class PayHistoryRetrieveTests : RepoTestsBase
    {
        private readonly IPayHistoryRepository _payHistoryRepo;

        public PayHistoryRetrieveTests()
        {
            _payHistoryRepo = new PayHistoryRepository(ctx, logger);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(14, 1)]
        [InlineData(15, 1)]
        [InlineData(16, 1)]
        [InlineData(17, 1)]
        [InlineData(18, 2)]
        public async Task ShouldGetPayHistoriesForEachEmployee(int employeeID, int numberOfRecords)
        {
            var payHistories = await _payHistoryRepo.GetPayHistories(employeeID, new PayHistoryParameters { PageNumber = 1, PageSize = 10 });
            var count = payHistories.Count;
            Assert.Equal(count, numberOfRecords);
        }

        [Theory]
        [InlineData(1, "2009-01-14", 125.5000)]
        [InlineData(14, "2008-12-29", 40.8654)]
        [InlineData(15, "2008-12-29", 40.8654)]
        [InlineData(16, "2008-01-24", 32.6923)]
        [InlineData(17, "2008-01-06", 32.6923)]
        [InlineData(18, "2008-01-31", 40.0000)]
        [InlineData(18, "2010-11-03", 63.4615)]
        public async Task ShouldRetrieveEachEmployeePayHistoryRecord(int employeeID, string rateChangeDate, decimal rate)
        {
            var payHistory = await _payHistoryRepo.GetPayHistoryByID(employeeID, DateTime.Parse(rateChangeDate));
            Assert.Equal(rate, payHistory.Rate);
        }
    }
}