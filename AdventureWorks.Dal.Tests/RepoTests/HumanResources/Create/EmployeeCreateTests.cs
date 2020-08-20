using System;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.DomainModels;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Create
{
    [Collection("AdventureWorks.Dal")]
    public class EmployeeCreateTests : RepoTestsBase
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeCreateTests()
        {
            _employeeRepo = new EmployeeRepository(ctx, logger);
        }

        [Fact]
        public void ShouldCreatePersonAndEmployeeFromAEmployeeDomainObj()
        {
            var employeeDomainObj = new EmployeeDomainObj
            {
                PersonType = "EM",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Jane",
                MiddleName = "Janice",
                LastName = "Doe",
                EmailPromotion = EmailPromoPreference.NoPromotions,
                EmailAddress = "jane.doe@adventure-works.com",
                PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                PasswordSalt = "bE3XiWw=",
                NationalIDNumber = "123456123",
                LoginID = "adventure-works\\jane1",
                JobTitle = "Design Engineer",
                BirthDate = new DateTime(1989, 6, 5),
                MaritalStatus = "S",
                Gender = "F",
                HireDate = new DateTime(2019, 10, 5),
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            _employeeRepo.CreateEmployee(employeeDomainObj);

            var result = _employeeRepo.GetEmployeeByID(employeeDomainObj.BusinessEntityID);
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateEmployeeLogin()
        {
            var employeeDomainObj = new EmployeeDomainObj
            {
                PersonType = "EM",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Jane",
                MiddleName = "Janice",
                LastName = "Doe",
                EmailPromotion = EmailPromoPreference.NoPromotions,
                EmailAddress = "jane.doe@adventure-works.com",
                PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                PasswordSalt = "bE3XiWw=",
                NationalIDNumber = "123456123",
                LoginID = "adventure-works\\ken0",
                JobTitle = "Design Engineer",
                BirthDate = new DateTime(1989, 6, 5),
                MaritalStatus = "S",
                Gender = "F",
                HireDate = new DateTime(2019, 10, 5),
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            Action testCode = () =>
            {
                _employeeRepo.CreateEmployee(employeeDomainObj);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate employee login!", exception.Message);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateEmployeeNationalIDNumber()
        {
            var employeeDomainObj = new EmployeeDomainObj
            {
                PersonType = "EM",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Jane",
                MiddleName = "Janice",
                LastName = "Doe",
                EmailPromotion = EmailPromoPreference.NoPromotions,
                EmailAddress = "jane.doe@adventure-works.com",
                PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                PasswordSalt = "bE3XiWw=",
                NationalIDNumber = "295847284",
                LoginID = "adventure-works\\jdoe",
                JobTitle = "Design Engineer",
                BirthDate = new DateTime(1989, 6, 5),
                MaritalStatus = "S",
                Gender = "F",
                HireDate = new DateTime(2019, 10, 5),
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            Action testCode = () =>
            {
                _employeeRepo.CreateEmployee(employeeDomainObj);
            };

            var exception = Assert.Throws<AdventureWorksUniqueIndexException>(testCode);
            Assert.Equal("Error: This operation would result in a duplicate employee national ID number!", exception.Message);
        }
    }
}