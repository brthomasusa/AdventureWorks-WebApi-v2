using System.Threading.Tasks;
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
        public async Task ShouldCreateOneDepartmentRecord()
        {
            var dept = new Department
            {
                Name = "Test",
                GroupName = "Test Groupname"
            };

            await _deptRepo.CreateDepartment(dept);

            var result = _deptRepo.GetDepartmentByID(dept.DepartmentID);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldRaiseExceptionDuplicateDepartment()
        {
            var dept = new Department
            {
                Name = "Finance",
                GroupName = "Executive General and Administration"
            };

            var exception = await Assert.ThrowsAsync<AdventureWorksUniqueIndexException>(() => _deptRepo.CreateDepartment(dept));
            Assert.Equal("Error: Create department failed; there is already a department named 'Finance'.", exception.Message);
        }
    }
}