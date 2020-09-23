using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.HumanResources;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Create
{
    [Collection("AdventureWorks.Dal")]
    public class PayHistoryCreateTests : RepoTestsBase
    {
        private readonly IPayHistoryRepository _payHistoryRepo;

        public PayHistoryCreateTests()
        {
            _payHistoryRepo = new PayHistoryRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldCreateEmployeePayHistoryRecord()
        {
            var payHistory = new EmployeePayHistory
            {
                BusinessEntityID = 1,
                RateChangeDate = new DateTime(2020, 8, 18),
                Rate = 150.00M,
                PayFrequency = PayFrequency.Biweekly
            };

            await _payHistoryRepo.CreatePayHistory(payHistory);

            var result = await _payHistoryRepo.GetPayHistoryByID(payHistory.BusinessEntityID, payHistory.RateChangeDate);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldRaiseExceptionDuplicateEmployeePayHistory()
        {
            var payHistory = new EmployeePayHistory
            {
                BusinessEntityID = 1,
                RateChangeDate = new DateTime(2009, 1, 14),
                Rate = 150.00M,
                PayFrequency = PayFrequency.Biweekly
            };

            var exception = await Assert.ThrowsAsync<AdventureWorksUniqueIndexException>(() => _payHistoryRepo.CreatePayHistory(payHistory));
            Assert.Equal("Error: This operation would result in a duplicate employee pay history record.", exception.Message);
        }
    }
}