using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Delete
{
    [Collection("AdventureWorks.Dal")]
    public class EmployeeDeleteTests : RepoTestsBase
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeDeleteTests()
        {
            _employeeRepo = new EmployeeRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldSetEmployeeIsActiveFlagToFalse()
        {
            var employeeID = 14;
            var employee = await _employeeRepo.GetEmployeeByID(employeeID);
            Assert.True(employee.IsActive);

            await _employeeRepo.DeleteEmployee(employee);

            var result = await _employeeRepo.GetEmployeeByID(employeeID);
            Assert.False(result.IsActive);
        }
    }
}