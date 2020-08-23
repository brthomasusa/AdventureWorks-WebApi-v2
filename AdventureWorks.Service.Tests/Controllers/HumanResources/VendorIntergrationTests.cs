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

        [Theory]
        [InlineData(2, "Cycles-R-Us")]
        [InlineData(3, "Desoto Bike Mart")]
        [InlineData(4, "DFW Bike Resellers")]
        [InlineData(5, "Light Speed")]
        [InlineData(6, "Trikes")]
        [InlineData(7, "Australia Bike Retailer")]
        public async Task ShouldGetOneVendorByID(int vendorID, string vendorName)
        {
            ResetDatabase();
            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}");
            Assert.True(httpResponse.IsSuccessStatusCode);
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var vendor = JsonConvert.DeserializeObject<VendorDomainObj>(jsonResponse);
            Assert.Equal(vendorName, vendor.Name);
        }

        [Theory]
        [InlineData(2, 1, 0)]
        [InlineData(3, 2, 1)]
        [InlineData(4, 0, 1)]
        [InlineData(5, 1, 1)]
        [InlineData(6, 1, 1)]
        [InlineData(7, 1, 3)]
        public async Task GetEachVendorDomainObjByIdWithDetails(int vendorID, int numberOfAddresses, int numberOfContacts)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/details");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var vendor = JsonConvert.DeserializeObject<VendorDomainObj>(jsonResponse);

            var addressCount = vendor.Addresses.Count;
            var contactCount = vendor.Contacts.Count;

            Assert.Equal(numberOfAddresses, addressCount);
            Assert.Equal(numberOfContacts, contactCount);
        }
    }
}