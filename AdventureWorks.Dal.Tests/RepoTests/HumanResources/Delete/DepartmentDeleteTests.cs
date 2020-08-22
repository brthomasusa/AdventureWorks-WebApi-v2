using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class DepartmentDeleteTests : RepoTestsBase
    {
        private readonly IDepartmentRepository _deptRepo;

        public DepartmentDeleteTests()
        {
            _deptRepo = new DepartmentRepository(ctx, logger);
        }

        [Fact]
        public void ShouldDeleteOneDepartmentRecord()
        {
            var deptID = 4;
            var dept = _deptRepo.GetDepartmentByID(deptID);
            Assert.NotNull(dept);

            _deptRepo.DeleteDepartment(dept);

            var result = _deptRepo.GetDepartmentByID(deptID);
            Assert.Null(result);
        }
    }
}