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
            _deptHistoryRepo = new DepartmentHistoryRepository(ctx);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(14, 0)]
        [InlineData(15, 1)]
        [InlineData(16, 1)]
        [InlineData(17, 1)]
        [InlineData(18, 2)]
        public void ShouldGetDeptHistoriesForEachEmployee(int employeeID, int numberOfRecords)
        {
            var records = _deptHistoryRepo.GetDepartmentHistories(employeeID, new DepartmentHistoryParameters { PageNumber = 1, PageSize = 10 });
            var count = records.Count;
            Assert.Equal(count, numberOfRecords);
        }

        [Theory]
        [InlineData(1, 10, 3)]
        [InlineData(15, 16, 3)]
        [InlineData(16, 10, 3)]
        [InlineData(17, 10, 3)]
        [InlineData(18, 14, 3)]
        [InlineData(18, 15, 3)]
        public void ShouldRetrieveEachDeptHistoryRecord(int employeeID, short deptID, byte shiftID)
        {
            var deptHistory = _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID);
            Assert.NotNull(deptHistory);
        }
    }
}