using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Service.Tests.Base;

namespace AdventureWorks.Service.Tests.Controllers.HumanResources
{
    [Collection("AdventureWorks.Service")]
    public class EmployeeValidationTests : BaseTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public EmployeeValidationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            rootAddress = "api/employees";
        }

        [Fact]
        public async Task ShouldFailDueToInvalidPersonType()
        {
            ResetDatabase();

            var employeeDomainObj = new EmployeeDomainObj
            {
                PersonType = "LM",
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

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailDueToInvalidMaritalStatus()
        {
            ResetDatabase();

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
                MaritalStatus = "m",
                Gender = "F",
                HireDate = new DateTime(2019, 10, 5),
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailDueToInvalidGenderCode()
        {
            ResetDatabase();

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
                MaritalStatus = "M",
                Gender = "MF",
                HireDate = new DateTime(2019, 10, 5),
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailDueToHireDateBefore19960701()
        {
            ResetDatabase();

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
                MaritalStatus = "M",
                Gender = "F",
                HireDate = new DateTime(1996, 6, 30),
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailDueToHireDateTooFarInFuture()
        {
            ResetDatabase();
            var invalidHireDate = (DateTime.Now).AddDays(2);

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
                MaritalStatus = "M",
                Gender = "F",
                HireDate = invalidHireDate,
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailDueToBirthDateBefore19300101()
        {
            ResetDatabase();

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
                BirthDate = new DateTime(1929, 12, 31),
                MaritalStatus = "M",
                Gender = "F",
                HireDate = new DateTime(2006, 6, 30),
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailDueToEmployeeLessThan18()
        {
            ResetDatabase();

            var invalidBirthDate = (DateTime.Now).AddDays(1);

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
                BirthDate = invalidBirthDate,
                MaritalStatus = "M",
                Gender = "F",
                HireDate = new DateTime(2006, 6, 30),
                IsSalaried = false,
                VacationHours = 10,
                SickLeaveHours = 10,
                IsActive = true
            };

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailDueToDuplicateNationalIDNumber()
        {
            ResetDatabase();

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
                NationalIDNumber = "998320692",
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

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailDueToDuplicateLoginID()
        {
            ResetDatabase();

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
                LoginID = "adventure-works\\jossef0",
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

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}