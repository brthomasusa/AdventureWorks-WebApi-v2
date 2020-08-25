using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Service.Tests.Base;

namespace AdventureWorks.Service.Tests.Controllers.HumanResources
{
    public class ContactIntergrationTests : BaseTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public ContactIntergrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            rootAddress = "api/vendors";
        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 1)]
        [InlineData(4, 1)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 3)]
        public async Task ShouldGetAllContactsForOneVendor(int vendorID, int numberOfContacts)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var contacts = JsonConvert.DeserializeObject<IList<ContactDomainObj>>(jsonResponse);
            var count = contacts.Count;

            Assert.Equal(numberOfContacts, count);
        }

        [Theory]
        [InlineData(8, "j.dough@adventure-works.com")]
        [InlineData(9, "jo3@adventure-works.com")]
        [InlineData(10, "terri.phide@adventure-works.com")]
        [InlineData(11, "marie0@adventure-works.com")]
        [InlineData(12, "william3@adventure-works.com")]
        [InlineData(13, "paula2@adventure-works.com")]
        [InlineData(19, "charelene0@adventure-works.com")]
        public async Task ShouldGetEachVendorContact(int contactID, string emailAddress)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/0/contact/{contactID}");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var contact = JsonConvert.DeserializeObject<ContactDomainObj>(jsonResponse);

            Assert.Equal(emailAddress, contact.EmailAddress);
        }

        [Theory]
        [InlineData(8, 0)]
        [InlineData(9, 1)]
        [InlineData(10, 2)]
        [InlineData(11, 1)]
        [InlineData(12, 1)]
        [InlineData(13, 1)]
        [InlineData(19, 1)]
        public async Task ShouldGetEachVendorContactWithPhoneRecords(int contactID, int numberOfPhones)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/0/contact/{contactID}/details");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var contact = JsonConvert.DeserializeObject<ContactDomainObj>(jsonResponse);
            var count = contact.Phones.Count;

            Assert.Equal(numberOfPhones, count);
        }
    }
}