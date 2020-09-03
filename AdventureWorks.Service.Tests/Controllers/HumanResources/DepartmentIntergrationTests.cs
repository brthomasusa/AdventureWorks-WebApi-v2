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
    public class DepartmentIntergrationTests : BaseTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public DepartmentIntergrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            rootAddress = "api/department";
        }

        [Fact]
        public async Task ShouldGetAllDepartmentsFilteredByQueryString()
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}?pageNumber=1&pageSize=2");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var departments = JsonConvert.DeserializeObject<IList<Department>>(jsonResponse);
            var count = departments.Count;

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task ShouldGetAllDepartmentsWithoutQueryString()
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var departments = JsonConvert.DeserializeObject<IList<Department>>(jsonResponse);
            var count = departments.Count;

            Assert.Equal(10, count);
        }

        [Fact]
        public async Task ShouldGetOneDepartmentRecord()
        {
            ResetDatabase();

            var departmentID = 16;
            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{departmentID}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var department = JsonConvert.DeserializeObject<Department>(jsonResponse);

            Assert.Equal("Research and Development", department.Name);
        }

        [Fact]
        public async Task ShouldFailToGetOneDepartmentRecordWithInvalidDeptID()
        {
            ResetDatabase();

            var departmentID = -1;
            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{departmentID}");

            Assert.False(httpResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task ShouldCreateOneDepartmentRecord()
        {
            ResetDatabase();

            var department = new Department
            {
                Name = "Software Development",
                GroupName = "Cloud Services"
            };

            string jsonDepartment = JsonConvert.SerializeObject(department);
            HttpContent content = new StringContent(jsonDepartment, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.True(response.IsSuccessStatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Department>(jsonResponse);

            Assert.Equal(department.Name, result.Name);
        }

        [Fact]
        public async Task ShouldFailToCreateOneDepartmentRecordWithDuplicateName()
        {
            ResetDatabase();

            var department = new Department
            {
                Name = "Human Resources",
                GroupName = "Executive General and Administration"
            };

            string jsonDepartment = JsonConvert.SerializeObject(department);
            HttpContent content = new StringContent(jsonDepartment, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToCreateOneDepartmentRecordWithDupeNameChangedCase()
        {
            ResetDatabase();

            var department = new Department
            {
                Name = "human reSources",
                GroupName = "Executive General and Administration"
            };

            string jsonDepartment = JsonConvert.SerializeObject(department);
            HttpContent content = new StringContent(jsonDepartment, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldUpdateOneDepartmentRecord()
        {
            ResetDatabase();

            var department = new Department
            {
                DepartmentID = 7,
                Name = "Human Resources Dept",
                GroupName = "Executive General and Administration"
            };

            string jsonDepartment = JsonConvert.SerializeObject(department);
            HttpContent content = new StringContent(jsonDepartment, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}", content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ShouldUpdateOneDepartmentRecordUsingSameName()
        {
            ResetDatabase();

            var department = new Department
            {
                DepartmentID = 7,
                Name = "Human Resources",
                GroupName = "Executive General and Administration"
            };

            string jsonDepartment = JsonConvert.SerializeObject(department);
            HttpContent content = new StringContent(jsonDepartment, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}", content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }


        [Fact]
        public async Task ShouldFailToUpdateOneDepartmentRecordDupeName()
        {
            ResetDatabase();

            var department = new Department
            {
                DepartmentID = 7,
                Name = "Finance",
                GroupName = "Executive General and Administration"
            };

            string jsonDepartment = JsonConvert.SerializeObject(department);
            HttpContent content = new StringContent(jsonDepartment, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDeleteOneDepartmentRecord()
        {
            ResetDatabase();

            var department = new Department
            {
                DepartmentID = 7,
                Name = "Finance",
                GroupName = "Executive General and Administration"
            };

            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}", department);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToDeleteOneDepartmentRecordWithChildRecords()
        {
            ResetDatabase();

            var department = new Department
            {
                DepartmentID = 10,
                Name = "Executive",
                GroupName = "Executive General and Administration"
            };

            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}", department);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToDeleteOneDepartmentRecordWithInvalidDeptID()
        {
            ResetDatabase();

            var department = new Department
            {
                DepartmentID = 77,
                Name = "Finance",
                GroupName = "Executive General and Administration"
            };

            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}", department);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}