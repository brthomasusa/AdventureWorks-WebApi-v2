using System;
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
        public void ShouldCreateEmployeePayHistoryRecord()
        {
            var payHistory = new EmployeePayHistory
            {
                BusinessEntityID = 1,
                RateChangeDate = new DateTime(2020, 8, 18),
                Rate = 150.00M,
                PayFrequency = PayFrequency.Biweekly
            };

            _payHistoryRepo.CreatePayHistory(payHistory);

            var result = _payHistoryRepo.GetPayHistoryByID(payHistory.BusinessEntityID, payHistory.RateChangeDate);
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateEmployeePayHistory()
        {
            var payHistory = new EmployeePayHistory
            {
                BusinessEntityID = 1,
                RateChangeDate = new DateTime(2009, 1, 14),
                Rate = 150.00M,
                PayFrequency = PayFrequency.Biweekly
            };

            Action testCode = () =>
            {
                _payHistoryRepo.CreatePayHistory(payHistory);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate employee pay history record.", exception.Message);
        }
    }
}