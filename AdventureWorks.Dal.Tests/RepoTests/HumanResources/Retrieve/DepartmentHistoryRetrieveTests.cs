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
    public class DepartmentHistoryRetrieveTests : RepoTestsBase
    {
        private readonly IDepartmentHistoryRepository _deptHistoryRepo;

        public DepartmentHistoryRetrieveTests()
        {
            _deptHistoryRepo = new DepartmentHistoryRepository(ctx, logger);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(14, 0)]
        [InlineData(15, 1)]
        [InlineData(16, 1)]
        [InlineData(17, 1)]
        [InlineData(18, 2)]
        public async Task ShouldGetDeptHistoriesForEachEmployee(int employeeID, int numberOfRecords)
        {
            var departmentHistoryParameters = new DepartmentHistoryParameters { PageNumber = 1, PageSize = 10 };
            var records = await _deptHistoryRepo.GetDepartmentHistories(employeeID, departmentHistoryParameters);
            var count = records.Count;
            Assert.Equal(count, numberOfRecords);
        }

        [Theory]
        [InlineData(1, 10, 3, "2009-01-14")]
        [InlineData(15, 16, 3, "2008-12-29")]
        [InlineData(16, 10, 3, "2008-01-24")]
        [InlineData(17, 10, 3, "2008-01-06")]
        [InlineData(18, 14, 3, "2008-01-31")]
        [InlineData(18, 15, 3, "2010-11-03")]
        public void ShouldRetrieveEachDeptHistoryRecord(int employeeID, short deptID, byte shiftID, string startDate)
        {
            var deptHistory = _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID, DateTime.Parse(startDate));
            Assert.NotNull(deptHistory);
        }
    }
}