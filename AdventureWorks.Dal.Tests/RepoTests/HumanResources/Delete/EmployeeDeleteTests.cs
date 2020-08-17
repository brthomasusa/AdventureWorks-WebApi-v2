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
            _employeeRepo = new EmployeeRepository(ctx);
        }

        [Fact]
        public void ShouldSetEmployeeIsActiveFlagToFalse()
        {
            var employeeID = 14;
            var employee = _employeeRepo.GetEmployeeByID(employeeID);
            Assert.True(employee.IsActive);

            _employeeRepo.DeleteEmployee(employee);

            var result = _employeeRepo.GetEmployeeByID(employeeID);
            Assert.False(result.IsActive);
        }
    }
}