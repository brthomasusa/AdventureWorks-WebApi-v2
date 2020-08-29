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

namespace AdventureWorks.Service.Tests.Controllers.Purchasing
{
    [Collection("AdventureWorks.Service")]
    public class AddressIntergrationTests : BaseTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public AddressIntergrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            rootAddress = "api/vendors";
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 0)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 1)]
        public async Task ShouldGetAllAddressesForOneVendor(int vendorID, int numberOfAddresses)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/addresses");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var addresses = JsonConvert.DeserializeObject<IList<AddressDomainObj>>(jsonResponse);
            var count = addresses.Count;

            Assert.Equal(numberOfAddresses, count);
        }

        [Theory]
        [InlineData(1, "98225")]
        [InlineData(2, "94704")]
        [InlineData(3, "91977")]
        [InlineData(4, "75123")]
        [InlineData(5, "75115")]
        [InlineData(6, "75231")]
        public async Task ShouldGetEachVendorAddresses(int addressID, string postalCode)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/address/{addressID}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var address = JsonConvert.DeserializeObject<AddressDomainObj>(jsonResponse);

            Assert.Equal(postalCode, address.PostalCode);
        }

        [Fact]
        public async Task ShouldCreateAddressFromAddressDomainObj()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9,
                PostalCode = "12345",
                AddressTypeID = 3,
                ParentEntityID = 4
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.True(response.IsSuccessStatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AddressDomainObj>(jsonResponse);

            Assert.Equal(address.AddressLine1, result.AddressLine1);
        }

        [Fact]
        public async Task ShouldFailToCreateAddressDueToInvalidAddressTypeID()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9,
                PostalCode = "12345",
                AddressTypeID = 93,
                ParentEntityID = 4
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToCreateAddressDueToInvalidParentEntityID()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9,
                PostalCode = "12345",
                AddressTypeID = 3,
                ParentEntityID = 43
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToCreateAddressDueToInvalidStateProvinceID()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9999,
                PostalCode = "12345",
                AddressTypeID = 3,
                ParentEntityID = 4
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldUpdateAddressFromAddressDomainObj()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressID = 1,
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9,
                PostalCode = "12345",
                AddressTypeID = 3,
                ParentEntityID = 7
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToUpdateAddressDueToInvalidAddressID()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressID = -1,
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9,
                PostalCode = "12345",
                AddressTypeID = 3,
                ParentEntityID = 7
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToUpdateAddressDueToInvalidAddressTypeID()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressID = 1,
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9,
                PostalCode = "12345",
                AddressTypeID = 63,
                ParentEntityID = 7
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToUpdateAddressDueToInvalidParentEntityID()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressID = 1,
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 9,
                PostalCode = "12345",
                AddressTypeID = 3,
                ParentEntityID = 77
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToUpdateAddressDueToInvalidStateProvinceID()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressID = 1,
                AddressLine1 = "123 Made-Up-Street Rd",
                AddressLine2 = "Ste 1,000,000",
                City = "Anywhere",
                StateProvinceID = 999,
                PostalCode = "12345",
                AddressTypeID = 3,
                ParentEntityID = 7
            };

            string jsonAddress = JsonConvert.SerializeObject(address);
            HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/address", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDeleteAddressFromAddressDomainObj()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressID = 1,
                AddressLine1 = "28 San Marino Ct",
                City = "Bellingham",
                StateProvinceID = 79,
                PostalCode = "98225",
                AddressTypeID = 3,
                ParentEntityID = 7
            };

            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}/address", address);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToDeleteAddressWithInvalidAddressID()
        {
            ResetDatabase();

            var address = new AddressDomainObj
            {
                AddressID = -1,
                AddressLine1 = "28 San Marino Ct",
                City = "Bellingham",
                StateProvinceID = 79,
                PostalCode = "98225",
                AddressTypeID = 3,
                ParentEntityID = 7
            };

            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}/address", address);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}