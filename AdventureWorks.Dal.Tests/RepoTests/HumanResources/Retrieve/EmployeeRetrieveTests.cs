using System.Threading.Tasks;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Retrieve
{
    [Collection("AdventureWorks.Dal")]
    public class EmployeeRetrieveTests : RepoTestsBase
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeRetrieveTests()
        {
            _employeeRepo = new EmployeeRepository(ctx, logger);
        }

        [Fact]
        public async Task ShouldGetAllEmployeeDomainObjs()
        {
            var employeeParams = new EmployeeParameters { PageNumber = 1, PageSize = 10 };
            var employees = await _employeeRepo.GetEmployees(employeeParams);
            var count = employees.Count;

            Assert.Equal(6, count);
        }

        [Theory]
        [InlineData(1, "ken@adventure-works.com")]
        [InlineData(14, "mr.mayhem@adventure-works.com")]
        [InlineData(15, "diane1@adventure-works.com")]
        [InlineData(16, "jossef@adventure-works.com")]
        [InlineData(17, "gail@adventure-works.com")]
        [InlineData(18, "terri@adventure-works.com")]
        public async Task ShouldRetrieveEachEmployeeDomainObj(int employeeID, string emailAddress)
        {
            var employeeDomainObj = await _employeeRepo.GetEmployeeByID(employeeID);
            Assert.Equal(emailAddress, employeeDomainObj.EmailAddress);
        }
    }
}