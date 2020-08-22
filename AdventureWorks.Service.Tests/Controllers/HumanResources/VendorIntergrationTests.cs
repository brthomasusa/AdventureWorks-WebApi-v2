using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Service.Tests.Base;

namespace AdventureWorks.Service.Tests.Controllers.HumanResources
{
    public class VendorIntergrationTests : BaseTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public VendorIntergrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            rootAddress = "api/vendors";
        }

        [Fact]
        public async Task ShouldGetAllVendors()
        {
            ResetDatabase();
            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var vendors = JsonConvert.DeserializeObject<List<VendorDomainObj>>(stringResponse);
            Assert.Contains(vendors, v => v.AccountNumber == "CYCLERU0001");
            Assert.Contains(vendors, v => v.AccountNumber == "DFWBIRE0001");
            Assert.Contains(vendors, v => v.AccountNumber == "AUSTRALI0001");
        }
    }
}