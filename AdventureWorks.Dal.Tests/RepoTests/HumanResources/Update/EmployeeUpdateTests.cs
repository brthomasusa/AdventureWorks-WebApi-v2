using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Update
{
    [Collection("AdventureWorks.Dal")]
    public class EmployeeUpdateTests : RepoTestsBase
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeUpdateTests()
        {
            _employeeRepo = new EmployeeRepository(ctx);
        }

        [Fact]
        public void ShouldUpdatePersonAndEmployeeUsingEmployeeDomainObj()
        {
            var employeeID = 1;
            var employee = _employeeRepo.GetEmployeeByID(employeeID);

            employee.Title = "Mr.";
            employee.MiddleName = "James";
            employee.EmailAddress = "kennethadventure-works.com";
            employee.LoginID = "adventure-works\\kenneth";

            _employeeRepo.UpdateEmployee(employee);

            var result = _employeeRepo.GetEmployeeByID(employeeID);

            Assert.Equal("Mr.", result.Title);
            Assert.Equal("James", result.MiddleName);
            Assert.Equal("kennethadventure-works.com", result.EmailAddress);
            Assert.Equal("adventure-works\\kenneth", result.LoginID);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateEmployeeLoginDuringUpdate()
        {
            var employeeID = 15;
            var employee = _employeeRepo.GetEmployeeByID(employeeID);
            employee.LoginID = "adventure-works\\terri0";

            Action testCode = () =>
            {
                _employeeRepo.UpdateEmployee(employee);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate employee login!", exception.Message);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateEmployeeNationalIDNumberDuringUpdate()
        {
            var employeeID = 15;
            var employee = _employeeRepo.GetEmployeeByID(employeeID);
            employee.NationalIDNumber = "245797967";

            Action testCode = () =>
            {
                _employeeRepo.UpdateEmployee(employee);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate employee national ID number!", exception.Message);
        }
    }
}