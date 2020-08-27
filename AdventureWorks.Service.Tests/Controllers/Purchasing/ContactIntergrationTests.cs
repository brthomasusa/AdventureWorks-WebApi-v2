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
        public async Task ShouldGetEachVendorContactByID(int contactID, string emailAddress)
        {
            ResetDatabase();

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/contact/{contactID}");
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

            var httpResponse = await _client.GetAsync($"{serviceAddress}{rootAddress}/contact/{contactID}/details");
            Assert.True(httpResponse.IsSuccessStatusCode);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var contact = JsonConvert.DeserializeObject<ContactDomainObj>(jsonResponse);
            var count = contact.Phones.Count;

            Assert.Equal(numberOfPhones, count);
        }

        [Fact]
        public async Task ShouldCreateOneVendorContactFromContactDomainObj()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = -1,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 17,
                ParentEntityID = 8,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/contact", content);

            Assert.True(response.IsSuccessStatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ContactDomainObj>(jsonResponse);

            Assert.Equal(contact.EmailAddress, result.EmailAddress);
        }

        [Fact]
        public async Task ShouldFailToCreateOneVendorDueToBadPersonType()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = -1,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 17,
                ParentEntityID = 8,
                PersonType = "EM",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/contact", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToCreateOneVendorContactDueToInvalidContactTypeID()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = 8,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 77,
                ParentEntityID = 3,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/contact", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToCreateOneVendorContactDueToInvalidParentEntityID()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = 8,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 17,
                ParentEntityID = 33,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{serviceAddress}{rootAddress}/contact", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }






















        [Fact]
        public async Task ShouldUpdateOneVendorContactFromContactDomainObj()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = 8,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 17,
                ParentEntityID = 3,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/contact", content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToUpdateOneVendorContactDueToInvalidContactID()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = 888,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 17,
                ParentEntityID = 3,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/contact", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToUpdateOneVendorContactDueToInvalidContactTypeID()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = 8,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 77,
                ParentEntityID = 3,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/contact", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToUpdateOneVendorContactDueToInvalidParentEntityID()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = 8,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 17,
                ParentEntityID = 33,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{serviceAddress}{rootAddress}/contact", content);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDeleteOneVendorContactFromContactDomainObj()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = 8,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 17,
                ParentEntityID = 3,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}/contact", contact);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ShouldFailToDeleteOneVendorContactDueToInvalidContactID()
        {
            ResetDatabase();

            var contact = new ContactDomainObj
            {
                BusinessEntityID = 200,
                EmailAddress = "testuser@adventure-works.com",
                EmailPasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                EmailPasswordSalt = "d2tgUmM=",
                ContactTypeID = 17,
                ParentEntityID = 3,
                PersonType = "VC",
                IsEasternNameStyle = false,
                Title = "Ms.",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                EmailPromotion = EmailPromoPreference.NoPromotions
            };

            string jsonVendor = JsonConvert.SerializeObject(contact);
            var response = await _client.DeleteAsJsonAsync($"{serviceAddress}{rootAddress}/contact", contact);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}