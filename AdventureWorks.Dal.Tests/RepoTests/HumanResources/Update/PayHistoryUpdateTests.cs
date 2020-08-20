using System;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Update
{
    [Collection("AdventureWorks.Dal")]
    public class PayHistoryUpdateTests : RepoTestsBase
    {
        private readonly IPayHistoryRepository _payHistoryRepo;

        public PayHistoryUpdateTests()
        {
            _payHistoryRepo = new PayHistoryRepository(ctx, logger);
        }

        [Fact]
        public void ShouldUpdateOneEmployeePayHistoryRecord()
        {
            var employeeID = 14;
            var rateChangeDate = "2008-12-29";
            var payHistory = _payHistoryRepo.GetPayHistoryByID(employeeID, DateTime.Parse(rateChangeDate));

            Assert.Equal(40.8654M, payHistory.Rate);

            payHistory.Rate = 75.0000M;

            _payHistoryRepo.UpdatePayHistory(payHistory);

            var result = _payHistoryRepo.GetPayHistoryByID(employeeID, DateTime.Parse(rateChangeDate));
            Assert.Equal(75.0000M, result.Rate);
        }
    }
}