using System;
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
            _deptHistoryRepo = new DepartmentHistoryRepository(ctx);
        }

        [Fact]
        public void ShouldDeleteOneEmployeeDepartmentHistoryRecord()
        {
            int employeeID = 1;
            short deptID = 10;
            byte shiftID = 3;
            var deptHistory = _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID);

            _deptHistoryRepo.DeleteDepartmentHistory(deptHistory);

            var result = _deptHistoryRepo.GetDepartmentHistoryByID(employeeID, deptID, shiftID);
            Assert.Null(result);
        }
    }
}