using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Create
{
    [Collection("AdventureWorks.Dal")]
    public class DepartmentHistoryCreateTests : RepoTestsBase
    {
        private readonly IDepartmentHistoryRepository _deptHistoryRepo;

        public DepartmentHistoryCreateTests()
        {
            _deptHistoryRepo = new DepartmentHistoryRepository(ctx, logger);
        }

        [Fact]
        public void ShouldCreateEmployeeDepartmentHistoryRecord()
        {
            var deptHistory = new EmployeeDepartmentHistory
            {
                BusinessEntityID = 14,
                DepartmentID = 10,
                ShiftID = 3,
                StartDate = new DateTime(2020, 8, 18)
            };

            _deptHistoryRepo.CreateDepartmentHistory(deptHistory);

            var result = _deptHistoryRepo.GetDepartmentHistoryByID(deptHistory.BusinessEntityID, deptHistory.DepartmentID, deptHistory.ShiftID);
            Assert.Equal(result.StartDate, new DateTime(2020, 8, 18));
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateEmployeeDepartmentHistory()
        {
            var deptHistory = new EmployeeDepartmentHistory
            {
                BusinessEntityID = 1,
                DepartmentID = 10,
                ShiftID = 3,
                StartDate = new DateTime(2009, 1, 14),
                EndDate = new DateTime(2009, 1, 15)
            };

            Action testCode = () =>
            {
                _deptHistoryRepo.CreateDepartmentHistory(deptHistory);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate employee department history record!", exception.Message);
        }
    }
}