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
        public async void ShouldGetAllVendorRecords()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendors = JsonConvert.DeserializeObject<List<Vendor>>(jsonResponse);
                Assert.Equal(3, vendors.Count);
            }
        }

        [Theory]
        [InlineData(2,"Light Speed")]
        [InlineData(3,"Trikes")]
        [InlineData(4,"Australia Bike Retailer")]    
        public async void ShouldGetOneVendorPerVendorID(int vendorID, string vendorName)
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

        [Fact]
        public async void ShouldGetAllVendorAddressViewModels()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/AddressViewModel");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendorAddresses = JsonConvert.DeserializeObject<List<VendorAddress>>(jsonResponse);
                Assert.Equal(3, vendorAddresses.Count);
            }            
        }

        [Theory]
        [InlineData(2, "Light Speed", "298 Sunnybrook Drive")]
        [InlineData(3, "Trikes", "90 Sunny Ave")]
        [InlineData(4, "Australia Bike Retailer", "28 San Marino Ct")]
        public async void ShouldGetAddressViewModelOfEachVendor(int vendorID, string name, string line1)
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/AddressViewModel/{vendorID}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendor = JsonConvert.DeserializeObject<VendorAddress>(jsonResponse);
                Assert.Equal(name, vendor.Name);
                Assert.Equal(line1, vendor.AddressLine1);
            }
        }

        [Fact]
        public async void ShouldFailToGetAddressViewModelOfEachVendor()
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/AddressViewModel/1");
                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }            
        }

        [Fact]
        public async void ShouldGetAllVendorContactViewModels()
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/ContactViewModel");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendorContacts = JsonConvert.DeserializeObject<List<VendorContact>>(jsonResponse);
                Assert.Equal(5, vendorContacts.Count);
            }             
        }

        [Theory]
        [InlineData(2,1)]
        [InlineData(3,1)]
        [InlineData(4,3)]
        public async void ShouldGetContactsForOneVendor(int vendorID, int numContacts)
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/ContactViewModel/{vendorID}");  
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendorContacts = JsonConvert.DeserializeObject<List<VendorContact>>(jsonResponse);
                Assert.Equal(numContacts, vendorContacts.Count);                
            }            
        }

        [Fact]
        public async void ShouldFailToGetContactViewModelOfVendor()
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/ContactViewModel/1");
                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }            
        }

        [Fact]
        public async void ShouldCreateOneVendor()
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                var vendor = new Vendor 
                {
                    AccountNumber = "TESTVEN0001",
                    Name = "Test Vendor",
                    CreditRating = CreditRating.Superior,
                    PreferredVendor = true,
                    IsActive = true                
                };                
                
                string jsonVendor = JsonConvert.SerializeObject(vendor);
                HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{serviceAddress}{rootAddress}/", content);
                
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Vendor>(jsonResponse);
                Assert.Equal(vendor.AccountNumber, result.AccountNumber);
            }             
        }

        [Fact]
        public async void ShouldUpdateOneVendor()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                int vendorID = 2;
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendor = JsonConvert.DeserializeObject<Vendor>(jsonResponse);
                Assert.Equal("Light Speed", vendor.Name);
                Assert.True(vendor.IsActive);
                Assert.True(vendor.PreferredVendor);
                Assert.Equal(CreditRating.Superior, vendor.CreditRating);

                vendor.PreferredVendor = false;
                vendor.IsActive = false;
                vendor.CreditRating = CreditRating.Average;

                string jsonVendor = JsonConvert.SerializeObject(vendor);
                HttpContent content = new StringContent(jsonVendor, Encoding.UTF8, "application/json");
                response = await client.PutAsync($"{serviceAddress}{rootAddress}/{vendorID}", content);  
                Assert.True(response.IsSuccessStatusCode);              
            }            
        }

        [Fact]
        public async void ShouldDeleteOneVendor()
        {
            // Deleting a vendor sets its IsActive flag to false, the vendor is not removed from the db

            ResetDatabase();

            using (var client = new HttpClient())
            {
                int vendorID = 2;
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendor = JsonConvert.DeserializeObject<Vendor>(jsonResponse);
                Assert.True(vendor.IsActive);

                response = await client.DeleteAsync($"{serviceAddress}{rootAddress}/{vendorID}");  
                Assert.True(response.IsSuccessStatusCode);  

                response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}");
                Assert.True(response.IsSuccessStatusCode);
                jsonResponse = await response.Content.ReadAsStringAsync();
                vendor = JsonConvert.DeserializeObject<Vendor>(jsonResponse);
                Assert.False(vendor.IsActive);                            
            }            
        }  

        [Fact]
        public async void ShouldGetAllAddressesForOneVendor()
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                int vendorID = 2;
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/address");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var addresses = JsonConvert.DeserializeObject<List<Address>>(jsonResponse);
                int count = addresses.Count;
                Assert.Equal(1, count);
                Assert.Equal(3, addresses[0].AddressID);
            }
        } 

        [Fact]
        public async void ShouldGetOneVendorAddress()
        {
            ResetDatabase();
            
            using (var client = new HttpClient())
            {
                int vendorID = 2;
                int addressID = 3;

                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/address/{addressID}");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var address = JsonConvert.DeserializeObject<Address>(jsonResponse);
                Assert.Equal("298 Sunnybrook Drive", address.AddressLine1);
            }            
        }

        [Fact]
        public async void ShouldAddVendorAddress()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var address = new Address
                {
                    AddressLine1 = "123 ASP.NET Core Hiway",
                    City = "Berkeley",
                    PostalCode = "94704",
                    StateProvinceID = 9
                };

                int vendorID = 2;
                int addressTypeID = 4;

                string jsonAddress = JsonConvert.SerializeObject(address);
                HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{serviceAddress}{rootAddress}/{vendorID}/address/{addressTypeID}", content);
                Assert.True(response.IsSuccessStatusCode); 

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Address>(jsonResponse);
                Assert.Equal(address.AddressLine1, result.AddressLine1);                               
            }
        }

        [Fact]
        public async void ShouldAddVendorContact()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var contact = new PersonClass
                {
                    PersonType = "VC",
                    IsEasternNameStyle = false,
                    FirstName = "Don",
                    LastName = "King",
                    EmailPromotion = EmailPromoPreference.AdventureWorksOnly,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "dking@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                        PasswordSalt = "d2tgUmM="
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "555-811-5555", PhoneNumberTypeID = 3}                        
                    }
                };

                var vendorID = 2;
                var contactTypeID = 19;

                string jsonContact = JsonConvert.SerializeObject(contact);
                HttpContent content = new StringContent(jsonContact, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact/{contactTypeID}", content);
                Assert.True(response.IsSuccessStatusCode); 

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonClass>(jsonResponse);
                Assert.Equal(contact.FirstName, result.FirstName);
                Assert.Equal(contact.LastName, result.LastName); 
            }
        }

        [Fact]
        public async void ShouldUpdateVendorContact()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                int vendorID = 4;
                int personID = 5;
                int contactTypeID = 17;

                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact/{personID}/{contactTypeID}");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<PersonClass>(jsonResponse);
                Assert.Equal("Jo", contact.FirstName);
                Assert.Equal("Zimmerman", contact.LastName);

                contact.FirstName = "Sally Sue";
                contact.LastName = "Zimmermanski";

                string jsonContact = JsonConvert.SerializeObject(contact);
                HttpContent content = new StringContent(jsonContact, Encoding.UTF8, "application/json");
                response = await client.PutAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact", content);
                Assert.True(response.IsSuccessStatusCode);

                response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact/{personID}/{contactTypeID}");
                Assert.True(response.IsSuccessStatusCode);

                jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonClass>(jsonResponse);
                Assert.Equal("Sally Sue", result.FirstName);
                Assert.Equal("Zimmermanski", result.LastName);                               
            } 
        }

        [Fact]
        public async void ShouldUpdateVendorAddress()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                int vendorID = 2;
                int addressID = 3;

                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/address/{addressID}");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var address = JsonConvert.DeserializeObject<Address>(jsonResponse);
                Assert.Equal("298 Sunnybrook Drive", address.AddressLine1);  

                address.AddressLine1 = "1 ASP.NET Core Center Dr.";  

                string jsonAddress = JsonConvert.SerializeObject(address);
                HttpContent content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
                response = await client.PutAsync($"{serviceAddress}{rootAddress}/{vendorID}/address", content);
                Assert.True(response.IsSuccessStatusCode); 

                response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/address/{addressID}");
                Assert.True(response.IsSuccessStatusCode);

                jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Address>(jsonResponse);
                Assert.Equal("1 ASP.NET Core Center Dr.", result.AddressLine1);                                          
            }
        }

        [Fact]
        public async void ShouldDeleteVendorAddress()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                int vendorID = 2;
                int addressID = 3;

                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/address/{addressID}");
                Assert.True(response.IsSuccessStatusCode);

                response = await client.DeleteAsync($"{serviceAddress}{rootAddress}/{vendorID}/address/{addressID}");
                Assert.True(response.IsSuccessStatusCode); 

                response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/address/{addressID}");
                Assert.False(response.IsSuccessStatusCode);                                     
            }            
        }

        [Fact]
        public async void ShouldDeleteVendorBusinessEntityContact()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                int vendorID = 4;
                int personID = 8;
                int contactTypeID = 18;

                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact/{personID}/{contactTypeID}");
                Assert.True(response.IsSuccessStatusCode);

                response = await client.DeleteAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact/{personID}/{contactTypeID}");
                Assert.True(response.IsSuccessStatusCode); 

                response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/contact/{personID}/{contactTypeID}");
                Assert.False(response.IsSuccessStatusCode);                                     
            }             
        }
    }
}