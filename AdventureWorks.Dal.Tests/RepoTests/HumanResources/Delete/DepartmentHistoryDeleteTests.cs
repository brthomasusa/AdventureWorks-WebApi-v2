using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class DepartmentHistoryDeleteTests : RepoTestsBase
    {
        private readonly IDepartmentHistoryRepository _deptHistoryRepo;

        public DepartmentHistoryDeleteTests()
        {
            _deptHistoryRepo = new DepartmentHistoryRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldDeleteOneEmployeeDepartmentHistoryRecord()
        {
            int employeeID = 1;
            short deptID = 10;
            byte shiftID = 3;
            DateTime startDate = new DateTime(2009, 1, 14);
            var deptHistory = await _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID, startDate);

            _deptHistoryRepo.DeleteDepartmentHistory(deptHistory);

            var result = await _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID, startDate);
            Assert.Null(result);
        }
    }
}