using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Service.Tests.Base;

namespace AdventureWorks.Service.Tests.Controllers.HumanResources
{
    [Collection("AdventureWorks.Service")]
    public class DeptHistoryIntergrationTests : BaseTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public DeptHistoryIntergrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            rootAddress = "api/employees";
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(14, 0)]
        [InlineData(15, 1)]
        [InlineData(16, 1)]
        [InlineData(17, 1)]
        [InlineData(18, 2)]
        public async Task ShouldGetAllDeptHistoryRecordsForEachEmployee(int employeeID, int numberOfRecords)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeID}/depthistory");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var deptHistories = JsonConvert.DeserializeObject<IList<EmployeeDepartmentHistory>>(jsonResponse);
            var count = deptHistories.Count;

            Assert.Equal(numberOfRecords, count);
        }

        [Fact]
        public async Task ShouldFailToGetAllPayHistoryRecordsDueToInvalidEmployeeID()
        {
            var employeeID = -1;
            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeID}/depthistory");

            Assert.False(httpResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Theory]
        [InlineData(1, 10, 3, "2009-01-14")]
        [InlineData(15, 16, 3, "2008-12-29")]
        [InlineData(16, 10, 3, "2008-01-24")]
        [InlineData(17, 10, 3, "2008-01-06")]
        [InlineData(18, 14, 3, "2008-01-31")]
        [InlineData(18, 15, 3, "2010-11-03")]
        public async Task ShouldGetEachDeptHistoryRecordByID(int employeeID, short deptID, byte shiftID, string startDate)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/depthistory/{employeeID}/{deptID}/{shiftID}/{startDate}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var deptHistory = JsonConvert.DeserializeObject<EmployeeDepartmentHistory>(jsonResponse);

            Assert.NotNull(deptHistory);
        }

        [Fact]
        public async Task ShouldFailToGetDeptHistoryRecordByIDWithInvalidID()
        {
            ResetDatabase();

            int employeeID = 1;
            short deptID = 2;
            byte shiftID = 3;
            string startDate = "2020-11-21";

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/depthistory/{employeeID}/{deptID}/{shiftID}/{startDate}");
            Assert.False(httpResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task ShouldCreateOneDeptHistoryRecord()
        {
            ResetDatabase();

            var deptHistory = new EmployeeDepartmentHistory
            {
                BusinessEntityID = 14,
                DepartmentID = 10,
                ShiftID = 3,
                StartDate = new DateTime(2019, 9, 2),
                EndDate = new DateTime(2020, 9, 2)
            };

            string jsonDeptHistory = JsonConvert.SerializeObject(deptHistory);
            HttpContent content = new StringContent(jsonDeptHistory, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/depthistory", content);

            Assert.True(response.IsSuccessStatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmployeeDepartmentHistory>(jsonResponse);

            Assert.Equal(deptHistory.EndDate, result.EndDate);
        }

        [Fact]
        public async Task ShouldFailToCreateOneDeptHistoryRecordWithInvalidEndDate()
        {
            ResetDatabase();

            var deptHistory = new EmployeeDepartmentHistory
            {
                BusinessEntityID = 14,
                DepartmentID = 10,
                ShiftID = 3,
                StartDate = new DateTime(2019, 9, 2),
                EndDate = new DateTime(2018, 9, 2)
            };

            string jsonDeptHistory = JsonConvert.SerializeObject(deptHistory);
            HttpContent content = new StringContent(jsonDeptHistory, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/depthistory", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldUpdateOneDeptHistoryRecord()
        {
            ResetDatabase();

            var deptHistory = new EmployeeDepartmentHistory
            {
                BusinessEntityID = 15,
                DepartmentID = 16,
                ShiftID = 3,
                StartDate = new DateTime(2008, 12, 29),
                EndDate = new DateTime(2018, 12, 29)
            };

            string jsonDeptHistory = JsonConvert.SerializeObject(deptHistory);
            HttpContent content = new StringContent(jsonDeptHistory, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/depthistory", content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToUpdateOneDeptHistoryRecord()
        {
            ResetDatabase();

            var deptHistory = new EmployeeDepartmentHistory
            {
                BusinessEntityID = 15,
                DepartmentID = 16,
                ShiftID = 3,
                StartDate = new DateTime(2017, 1, 29),
                EndDate = new DateTime(2018, 12, 29)
            };

            string jsonDeptHistory = JsonConvert.SerializeObject(deptHistory);
            HttpContent content = new StringContent(jsonDeptHistory, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/depthistory", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task ShouldDeleteOneDeptHistoryRecord()
        {
            ResetDatabase();

            var deptHistory = new EmployeeDepartmentHistory
            {
                BusinessEntityID = 15,
                DepartmentID = 16,
                ShiftID = 3,
                StartDate = new DateTime(2008, 12, 29),
                EndDate = new DateTime(2018, 12, 29)
            };

            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}/depthistory", deptHistory);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToDeleteOneDeptHistoryRecordBadPrimaryKeys()
        {
            ResetDatabase();

            var deptHistory = new EmployeeDepartmentHistory
            {
                BusinessEntityID = 15,
                DepartmentID = 16,
                ShiftID = 3,
                StartDate = new DateTime(2018, 12, 29),
                EndDate = new DateTime(2018, 12, 29)
            };

            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}/depthistory", deptHistory);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}