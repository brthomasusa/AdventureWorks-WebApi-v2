using System;
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
        public void ShouldUpdateEmployeeDepartmentHistoryRecord()
        {
            int employeeID = 1;
            short deptID = 10;
            byte shiftID = 3;
            var deptHistory = _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID);

            var endDate = new DateTime(2019, 1, 1);
            deptHistory.EndDate = endDate;

            _deptHistoryRepo.UpdateDepartmentHistory(deptHistory);

            var result = _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID);
            Assert.Equal(endDate, result.EndDate);
        }
    }
}