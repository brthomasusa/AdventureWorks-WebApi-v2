using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Xunit;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.ViewModel;
using AdventureWorks.Service.Tests.Base;
using PersonClass = AdventureWorks.Models.Person.Person;

namespace AdventureWorks.Service.Tests.Purchasing
{
    [Collection("AdventureWorks.Service")]
    public class VendorControllerTests : BaseTest
    {
        public VendorControllerTests()
        {
            rootAddress = "api/vendor";
        }
        
        [Fact]
        public async void ShouldGetAllVendors()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendors = JsonConvert.DeserializeObject<List<Vendor>>(jsonResponse);
                Assert.Equal(6, vendors.Count);
            }
        }

        [Theory]
        [InlineData(2,"Cycles-R-Us")]
        [InlineData(3,"Desoto Bike Mart")]
        [InlineData(4,"DFW Bike Resellers")]  
        [InlineData(5,"Light Speed")]
        [InlineData(6,"Trikes")]
        [InlineData(7,"Australia Bike Retailer")]           
        public async void ShouldGetVendorByID(int vendorID, string vendorName)
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendor = JsonConvert.DeserializeObject<Vendor>(jsonResponse);
                Assert.Equal(vendorName, vendor.Name);
            }
        }

        [Theory]
        [InlineData(2,1)]
        [InlineData(3,2)]
        [InlineData(4,0)]
        [InlineData(5,1)]
        [InlineData(6,1)]        
        [InlineData(7,1)]
        public async void ShouldGetAllAddressesForOneVendor(int vendorID, int numAddresses)
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/address");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var addresses = JsonConvert.DeserializeObject<List<Address>>(jsonResponse);
                Assert.Equal(numAddresses, addresses.Count);  
            }
        } 

        [Fact]
        public async void ShouldGetOneVendorAddress()
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                int addressID = 6;

                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/address/{addressID}");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var address = JsonConvert.DeserializeObject<Address>(jsonResponse);
                Assert.Equal("6266 Melody Lane", address.AddressLine1);
            }            
        }

        [Theory]
        [InlineData(2,0)]
        [InlineData(3,1)]
        [InlineData(4,1)]
        [InlineData(7,3)]
        public async void ShouldGetAllContactsForEachVendor(int vendorID, int numContacts)
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact");  
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendorContacts = JsonConvert.DeserializeObject<List<PersonClass>>(jsonResponse);
                Assert.Equal(numContacts, vendorContacts.Count);                
            }            
        }

        [Fact]
        public async void ShouldGetOneVendorContact()
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                int personID = 8;
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/contact/{personID}");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<PersonClass>(jsonResponse);
                Assert.Equal("Johnny", contact.FirstName);
                Assert.Equal("Dough", contact.LastName);
            }            
        }



    }
}