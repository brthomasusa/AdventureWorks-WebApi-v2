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
    public class DepartmentCreateTests : RepoTestsBase
    {
        private readonly IDepartmentRepository _deptRepo;

        public DepartmentCreateTests()
        {
            _deptRepo = new DepartmentRepository(ctx, logger);
        }

        [Fact]
        public void ShouldCreateOneDepartmentRecord()
        {
            var dept = new Department
            {
                Name = "Test",
                GroupName = "Test Groupname"
            };

            _deptRepo.CreateDepartment(dept);

            var result = _deptRepo.GetDepartmentByID(dept.DepartmentID);
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateDepartment()
        {
            var dept = new Department
            {
                Name = "Finance",
                GroupName = "Executive General and Administration"
            };

            Action testCode = () =>
            {
                _deptRepo.CreateDepartment(dept);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate HR department!", exception.Message);
        }
    }
}