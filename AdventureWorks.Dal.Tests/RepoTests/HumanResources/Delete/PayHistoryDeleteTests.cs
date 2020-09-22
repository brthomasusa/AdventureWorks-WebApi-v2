using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class PayHistoryDeleteTests : RepoTestsBase
    {
        private readonly IPayHistoryRepository _payHistoryRepo;

        public PayHistoryDeleteTests()
        {
            _payHistoryRepo = new PayHistoryRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldDeleteOneEmployeePayHistoryRecord()
        {
            var employeeID = 18;
            var rateChangeDate = "2008-01-31";
            var payHistory = await _payHistoryRepo.GetPayHistoryByID(employeeID, DateTime.Parse(rateChangeDate));

            Assert.NotNull(payHistory);

            await _payHistoryRepo.DeletePayHistory(payHistory);

            payHistory = await _payHistoryRepo.GetPayHistoryByID(employeeID, DateTime.Parse(rateChangeDate));
            Assert.Null(payHistory);
        }
    }
}