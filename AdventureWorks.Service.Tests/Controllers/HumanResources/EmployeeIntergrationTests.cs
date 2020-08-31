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
    public class EmployeeIntergrationTests : BaseTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public EmployeeIntergrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            rootAddress = "api/employees";
        }

        [Fact]
        public async Task ShouldGetAllEmployeeDomainObjects()
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<IList<EmployeeDomainObj>>(jsonResponse);
            var count = employees.Count;

            Assert.Equal(6, count);
        }

        [Theory]
        [InlineData(1, "295847284")]
        [InlineData(14, "147994146")]
        [InlineData(15, "811994146")]
        [InlineData(16, "998320692")]
        [InlineData(17, "695256908")]
        [InlineData(18, "245797967")]
        public async Task ShouldGetEachEmployeeDomainObject(int employeeID, string nationalIdNumber)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeID}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<EmployeeDomainObj>(jsonResponse);

            Assert.Equal(nationalIdNumber, employee.NationalIDNumber);
        }

        [Fact]
        public async Task ShouldFailToGetEmployeeDomainObjectByID()
        {
            ResetDatabase();

            var employeeID = -1;
            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeID}");

            Assert.False(httpResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(14, 0, 0, 1)]
        [InlineData(15, 1, 1, 1)]
        [InlineData(16, 1, 1, 1)]
        [InlineData(17, 1, 1, 1)]
        [InlineData(18, 1, 2, 2)]
        public async Task ShouldGetDetailsForEachEmployeeDomainObject(int employeeID, int numberOfAddresses, int numberOfDeptHist, int numberOfPayHist)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeID}/details");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<EmployeeDomainObj>(jsonResponse);

            var addressCount = employee.Addresses.Count;
            var deptHistCount = employee.DepartmentHistories.Count;
            var payHistCount = employee.PayHistories.Count;

            Assert.Equal(numberOfAddresses, addressCount);
            Assert.Equal(numberOfDeptHist, deptHistCount);
            Assert.Equal(numberOfPayHist, payHistCount);
        }

        [Fact]
        public async Task ShouldFailToGetEmployeeDomainObjectByIDWithDetails()
        {
            ResetDatabase();

            var employeeID = -1;
            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeID}/details");

            Assert.False(httpResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task ShouldCreateEmployeeFromEmployeeDomainObject()
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

            Assert.True(response.IsSuccessStatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmployeeDomainObj>(jsonResponse);

            Assert.Equal(employeeDomainObj.EmailAddress, result.EmailAddress);
        }

        [Fact]
        public async Task ShouldUpdateAnEmployeeDomainObject()
        {
            ResetDatabase();

            var employeeDomainObj = new EmployeeDomainObj
            {
                BusinessEntityID = 18,
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

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}", content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var result = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeDomainObj.BusinessEntityID}");
            var jsonResponse = await result.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<EmployeeDomainObj>(jsonResponse);

            Assert.Equal(employeeDomainObj.NationalIDNumber, employee.NationalIDNumber);
        }

        [Fact]
        public async Task ShouldFailToUpdateAnEmployeeDomainObjectWithInvalidID()
        {
            ResetDatabase();

            var employeeDomainObj = new EmployeeDomainObj
            {
                BusinessEntityID = -8,
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

            string jsonEmployee = JsonConvert.SerializeObject(employeeDomainObj);
            HttpContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task ShouldChangeEmployeeIsActiveFlagToFalse()
        {
            ResetDatabase();

            var employeeDomainObj = new EmployeeDomainObj
            {
                BusinessEntityID = 18,
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

            string jsonContact = JsonConvert.SerializeObject(employeeDomainObj);
            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}", employeeDomainObj);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var result = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeDomainObj.BusinessEntityID}");
            var jsonResponse = await result.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<EmployeeDomainObj>(jsonResponse);

            Assert.False(employee.IsActive);
        }

        [Fact]
        public async Task ShouldFailToChangeEmployeeIsActiveFlagToFalse()
        {
            ResetDatabase();

            var employeeDomainObj = new EmployeeDomainObj
            {
                BusinessEntityID = -1,
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

            string jsonContact = JsonConvert.SerializeObject(employeeDomainObj);
            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}", employeeDomainObj);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}