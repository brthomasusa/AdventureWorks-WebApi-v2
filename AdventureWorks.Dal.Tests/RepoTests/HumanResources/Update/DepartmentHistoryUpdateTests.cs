using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Update
{
    [Collection("AdventureWorks.Dal")]
    public class DepartmentHistoryUpdateTests : RepoTestsBase
    {
        private readonly IDepartmentHistoryRepository _deptHistoryRepo;

        public DepartmentHistoryUpdateTests()
        {
            _deptHistoryRepo = new DepartmentHistoryRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldUpdateEmployeeDepartmentHistoryRecord()
        {
            int employeeID = 1;
            short deptID = 10;
            byte shiftID = 3;
            DateTime startDate = new DateTime(2009, 1, 14);
            var deptHistory = await _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID, startDate);

            var endDate = new DateTime(2019, 1, 1);
            deptHistory.EndDate = endDate;

            _deptHistoryRepo.UpdateDepartmentHistory(deptHistory);

            var result = await _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID, startDate);
            Assert.Equal(endDate, result.EndDate);
        }
    }
}