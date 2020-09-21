using System;
using System.Threading.Tasks;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Update
{
    [Collection("AdventureWorks.Dal")]
    public class DepartmentUpdateTests : RepoTestsBase
    {
        private readonly IDepartmentRepository _deptRepo;

        public DepartmentUpdateTests()
        {
            _deptRepo = new DepartmentRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldUpdateOneDepartmentRecord()
        {
            var deptID = 12;
            var dept = await _deptRepo.GetDepartmentByID(deptID);
            Assert.Equal("Sales and Marketing", dept.GroupName);

            dept.GroupName = "Sales and Marketing and More!";
            _deptRepo.UpdateDepartment(dept);

            var result = await _deptRepo.GetDepartmentByID(deptID);
            Assert.Equal(dept.GroupName, result.GroupName);
        }
    }
}