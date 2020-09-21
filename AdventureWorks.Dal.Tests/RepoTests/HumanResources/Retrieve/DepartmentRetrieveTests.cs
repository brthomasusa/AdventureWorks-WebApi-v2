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
    public class DepartmentRetrieveTests : RepoTestsBase
    {
        private readonly IDepartmentRepository _deptRepo;

        public DepartmentRetrieveTests()
        {
            _deptRepo = new DepartmentRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldGetAllDepartments()
        {
            var departments = await _deptRepo.GetDepartments(new DepartmentParameters { PageNumber = 1, PageSize = 16 });
            var count = departments.Count;
            Assert.Equal(16, count);
        }
    }
}