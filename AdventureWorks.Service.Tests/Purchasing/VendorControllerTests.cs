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
        [InlineData(2, "Cycles-R-Us")]
        [InlineData(3, "Desoto Bike Mart")]
        [InlineData(4, "DFW Bike Resellers")]
        [InlineData(5, "Light Speed")]
        [InlineData(6, "Trikes")]
        [InlineData(7, "Australia Bike Retailer")]
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

        [Fact]
        public async void ShouldGetAllVendorViewModels()
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/ViewModels");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendors = JsonConvert.DeserializeObject<List<VendorViewModel>>(jsonResponse);
                Assert.Equal(6, vendors.Count);
            }
        }

        [Theory]
        [InlineData(2, "Cycles-R-Us")]
        [InlineData(3, "Desoto Bike Mart")]
        [InlineData(4, "DFW Bike Resellers")]
        [InlineData(5, "Light Speed")]
        [InlineData(6, "Trikes")]
        [InlineData(7, "Australia Bike Retailer")]
        public async void ShouldGetVendorViewModelByBizEntityID(int vendorID, string vendorName)
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/ViewModels/{vendorID}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendor = JsonConvert.DeserializeObject<VendorViewModel>(jsonResponse);
                Assert.Equal(vendorName, vendor.Name);
            }
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 0)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 1)]
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
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 0)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 1)]
        public async void ShouldGetAllAddressViewModelsForOneVendor(int vendorID, int numAddresses)
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/AddressViewModel");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var addresses = JsonConvert.DeserializeObject<List<AddressViewModel>>(jsonResponse);
                Assert.Equal(numAddresses, addresses.Count);
            }
        }

        [Theory]
        [InlineData(1, "28 San Marino Ct")]
        [InlineData(2, "90 Sunny Ave")]
        [InlineData(3, "298 Sunnybrook Drive")]
        [InlineData(4, "1900 Desoto Court")]
        [InlineData(5, "211 East Pleasant Run Rd")]
        [InlineData(6, "6266 Melody Lane")]
        public async void ShouldGetOneVendorAddressViewModel(int addressID, string line1)
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/AddressViewModel/{addressID}");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var address = JsonConvert.DeserializeObject<AddressViewModel>(jsonResponse);
                Assert.Equal(line1, address.AddressLine1);
            }
        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 1)]
        [InlineData(4, 1)]
        [InlineData(7, 3)]
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

        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 1)]
        [InlineData(4, 1)]
        [InlineData(7, 3)]
        public async void ShouldGetAllContactViewModelsForEachVendor(int vendorID, int numContacts)
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{vendorID}/ContactViewModel");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vendorContacts = JsonConvert.DeserializeObject<List<VendorContactViewModel>>(jsonResponse);
                Assert.Equal(numContacts, vendorContacts.Count);
            }
        }

        [Theory]
        [InlineData(8, "j.dough@adventure-works.com")]
        [InlineData(9, "jo3@adventure-works.com")]
        [InlineData(10, "terri.phide@adventure-works.com")]
        [InlineData(11, "marie0@adventure-works.com")]
        [InlineData(12, "william3@adventure-works.com")]
        [InlineData(13, "paula2@adventure-works.com")]
        [InlineData(19, "charelene0@adventure-works.com")]
        public async void ShouldGetOneVendorContactViewModel(int personID, string emailAddress)
        {
            ResetDatabase();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/ContactViewModel/{personID}");
                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<VendorContactViewModel>(jsonResponse);
                Assert.Equal(emailAddress, contact.EmailAddress);
            }
        }

        // TODO HttpPost tests
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

            var vendor = new Vendor
            {
                AccountNumber = "TESTVEN0001",
                Name = "Test Vendor",
                CreditRating = CreditRating.Superior,
                PreferredVendor = true,
                IsActive = true
            };

            using (var client = new HttpClient())
            {
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
        public async void ShouldAddVendorContactToVendor()
        {
            ResetDatabase();

            var vendorContact = new PersonClass
            {
                PersonType = "V8",
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
                    new PersonPhone {PhoneNumber = "555-111-5555", PhoneNumberTypeID = 3}
                }
            };

            using (var client = new HttpClient())
            {
                string jsonVendorContact = JsonConvert.SerializeObject(vendorContact);
                HttpContent content = new StringContent(jsonVendorContact, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{serviceAddress}{rootAddress}/3/VendorContact/17", content);

                Assert.True(response.IsSuccessStatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonClass>(jsonResponse);
                Assert.Equal(vendorContact.EmailAddressObj.PersonEmailAddress, result.EmailAddressObj.PersonEmailAddress);
            }
        }

        [Fact]
        public async void ShouldFailAddVendorContactBadPersonType()
        {
            ResetDatabase();

            var vendorContact = new PersonClass
            {
                PersonType = "V8",
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
                    new PersonPhone {PhoneNumber = "555-111-5555", PhoneNumberTypeID = 3}
                }
            };

            using (var client = new HttpClient())
            {
                string jsonVendorContact = JsonConvert.SerializeObject(vendorContact);
                HttpContent content = new StringContent(jsonVendorContact, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{serviceAddress}{rootAddress}/3/VendorContact/17", content);

                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }


        // TODO HttpPut tests

        // TODO HttpDelete tests
    }
}