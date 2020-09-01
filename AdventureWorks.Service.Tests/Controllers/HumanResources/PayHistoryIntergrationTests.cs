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
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Service.Tests.Base;

namespace AdventureWorks.Service.Tests.Controllers.HumanResources
{
    [Collection("AdventureWorks.Service")]
    public class PayHistoryIntergrationTests : BaseTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public PayHistoryIntergrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            rootAddress = "api/employees";
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(14, 1)]
        [InlineData(15, 1)]
        [InlineData(16, 1)]
        [InlineData(17, 1)]
        [InlineData(18, 2)]
        public async Task ShouldGetAllPayHistoryRecordsForEachEmployee(int employeeID, int numberOfRecords)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{employeeID}/payhistory");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var payHistories = JsonConvert.DeserializeObject<IList<EmployeePayHistory>>(jsonResponse);
            var count = payHistories.Count;

            Assert.Equal(numberOfRecords, count);
        }

        [Theory]
        [InlineData(1, "2009-01-14", 125.5000)]
        [InlineData(14, "2008-12-29", 40.8654)]
        [InlineData(15, "2008-12-29", 40.8654)]
        [InlineData(16, "2008-01-24", 32.6923)]
        [InlineData(17, "2008-01-06", 32.6923)]
        [InlineData(18, "2008-01-31", 40.0000)]
        [InlineData(18, "2010-11-03", 63.4615)]
        public async Task ShouldGetEachPayHistoryRecord(int employeeID, string ratechangedate, decimal rate)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/payhistory/{employeeID}/{ratechangedate}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var payHistory = JsonConvert.DeserializeObject<EmployeePayHistory>(jsonResponse);

            Assert.Equal(rate, payHistory.Rate);
        }

        [Fact]
        public async Task ShouldCreateOneEmployeePayHistoryRecord()
        {
            ResetDatabase();

            var payHistory = new EmployeePayHistory
            {
                BusinessEntityID = 1,
                RateChangeDate = new DateTime(2020, 8, 18),
                Rate = 150.00M,
                PayFrequency = PayFrequency.Biweekly
            };

            string jsonPayHistory = JsonConvert.SerializeObject(payHistory);
            HttpContent content = new StringContent(jsonPayHistory, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/payhistory", content);

            Assert.True(response.IsSuccessStatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmployeePayHistory>(jsonResponse);

            Assert.Equal(payHistory.Rate, result.Rate);
        }

        [Fact]
        public async Task ShouldUpdateOneEmployeePayHistoryRecord()
        {
            ResetDatabase();

            var payHistory = new EmployeePayHistory
            {
                BusinessEntityID = 17,
                RateChangeDate = new DateTime(2008, 1, 6),
                Rate = 50.00M,
                PayFrequency = PayFrequency.Biweekly
            };

            string jsonPayHistory = JsonConvert.SerializeObject(payHistory);
            HttpContent content = new StringContent(jsonPayHistory, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/payhistory", content);

            Assert.True(response.IsSuccessStatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmployeePayHistory>(jsonResponse);

            Assert.Equal(payHistory.Rate, result.Rate);
        }
    }
}